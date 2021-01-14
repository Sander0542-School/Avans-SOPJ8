using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.EmployeeRules;
using Bumbo.Logic.Utils;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchEmployee")]
    [Route("Branches/{branchId}/{controller}/{action=Week}")]
    public class ScheduleController : Controller
    {
        private readonly IStringLocalizer<ScheduleController> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly RepositoryWrapper _wrapper;

        public ScheduleController(RepositoryWrapper wrapper, IStringLocalizer<ScheduleController> localizer, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _localizer = localizer;
            _userManager = userManager;
        }

        [Route("{year?}/{week?}/{department?}")]
        public async Task<IActionResult> Week(int branchId, int? year, int? week, Department? department)
        {
            if (!year.HasValue || !week.HasValue)
            {
                return RedirectToAction(nameof(Week), new
                {
                    branchId,
                    year = year ?? DateTime.Today.Year,
                    week = week ?? ISOWeek.GetWeekOfYear(DateTime.Today)
                });
            }

            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            try
            {
                var departments = GetUserDepartments(User, branchId);

                if (department.HasValue)
                {
                    if (departments.Contains(department.Value))
                    {
                        departments = new[]
                        {
                            department.Value
                        };
                    }
                    else
                    {
                        return RedirectToAction(nameof(Week), new
                        {
                            branchId,
                            year,
                            week
                        });
                    }
                }

                var users = await _wrapper.User.GetUsersAndShifts(branch, year.Value, week.Value, departments);

                return View(new DepartmentViewModel
                {
                    Year = year.Value,
                    Week = week.Value,
                    Department = department,
                    Branch = branch,

                    ScheduleApproved = department.HasValue && users.Any(user => user.Shifts.Any(shift => shift.Schedule.Department == department.Value && shift.Schedule.Confirmed)),
                    EmployeeShifts = users.Select(user =>
                    {
                        var notifications = WorkingHours.ValidateWeek(user, year.Value, week.Value);

                        return new DepartmentViewModel.EmployeeShift
                        {
                            UserId = user.Id,
                            Name = UserUtil.GetFullName(user),
                            Contract = user.Contracts.FirstOrDefault()?.Function ?? "",
                            MaxHours = WorkingHours.MaxHoursPerWeek(user, year.Value, week.Value),
                            Scale = user.Contracts.FirstOrDefault()?.Scale ?? 0,
                            Shifts = user.Shifts.Select(shift =>
                            {
                                return new DepartmentViewModel.Shift
                                {
                                    Id = shift.Id,
                                    Department = shift.Schedule.Department,
                                    Date = shift.Date,
                                    StartTime = shift.StartTime,
                                    EndTime = shift.EndTime,
                                    Sick = shift.WorkedShift?.Sick ?? false,
                                    Notifications = notifications.First(pair => pair.Key.Id == shift.Id).Value
                                };
                            }).ToList()
                        };
                    }).ToList(),
                    EmployeeAvailability = users.ToDictionary(user => user.Id, user => user.UserAvailabilities.Select(availability => new DepartmentViewModel.Availability
                    {
                        DayOfWeek = availability.Day,
                        StartTime = availability.StartTime,
                        EndTime = availability.EndTime
                    }).ToList()),
                    InputShift = new DepartmentViewModel.InputShiftModel
                    {
                        Year = year.Value,
                        Week = week.Value,
                        Department = department
                    },
                    DeleteShift = new DepartmentViewModel.DeleteShiftModel
                    {
                        Year = year.Value,
                        Week = week.Value,
                        Department = department
                    },
                    InputCopyWeek = new DepartmentViewModel.InputCopyWeekModel
                    {
                        Year = year.Value,
                        Week = week.Value,
                        Department = department
                    },
                    InputApproveSchedule = new DepartmentViewModel.InputApproveScheduleModel
                    {
                        Year = year.Value,
                        Week = week.Value,
                        Department = department
                    }
                });
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveShift")]
        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> SaveShift(int branchId, DepartmentViewModel.InputShiftModel shiftModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            var alertMessage = $"danger:{_localizer["MessageShiftNotSaved"]}";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.Shift.Get(shift1 => shift1.Id == shiftModel.ShiftId);

                var newShift = true;

                if (shift == null)
                {
                    var schedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, shiftModel.Year, shiftModel.Week, shiftModel.Department.Value);

                    shift = new Shift
                    {
                        ScheduleId = schedule.Id,
                        UserId = shiftModel.UserId,
                        Date = shiftModel.Date,
                        StartTime = shiftModel.StartTime,
                        EndTime = shiftModel.EndTime
                    };
                }
                else
                {
                    shift.StartTime = shiftModel.StartTime;
                    shift.EndTime = shiftModel.EndTime;

                    newShift = false;
                }

                if (shiftModel.Sick)
                {
                    shift.WorkedShift ??= new WorkedShift();
                    shift.WorkedShift.StartTime = shiftModel.StartTime;
                    shift.WorkedShift.EndTime = shiftModel.EndTime;
                    shift.WorkedShift.Sick = shiftModel.Sick;
                }
                else
                {
                    if (shift.WorkedShift != null)
                    {
                        shift.WorkedShift.Sick = shiftModel.Sick;
                        if (shift.Date > DateTime.Today)
                        {
                            await _wrapper.WorkedShift.Remove(shift.WorkedShift);
                            shift.WorkedShift = null;
                        }
                    }
                }

                var success = (newShift ? await _wrapper.Shift.Add(shift) : await _wrapper.Shift.Update(shift)) != null;

                if (success)
                {
                    alertMessage = $"success:{_localizer["MessageShiftSaved"]}";
                }
            }

            TempData["AlertMessage"] = alertMessage;

            return RedirectToAction(nameof(Week), new
            {
                branchId,
                year = shiftModel.Year,
                week = shiftModel.Week,
                department = shiftModel.Department
            });
        }

        [HttpPost]
        [Route("DeleteShift")]
        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> DeleteShift(int branchId, DepartmentViewModel.DeleteShiftModel deleteModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            var alertMessage = $"danger:{_localizer["MessageShiftNotDeleted"]}";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.Shift.Get(shift1 => shift1.Id == deleteModel.ShiftId);

                if (shift != null)
                {
                    if (shift.WorkedShift == null)
                    {
                        if (await _wrapper.Shift.Remove(shift) != null)
                        {
                            alertMessage = $"success:{_localizer["MessageShiftDeleted"]}";
                        }
                    }
                    else
                    {
                        alertMessage = $"danger:{_localizer["MessageShiftContainsWorkedShift"]}";
                    }
                }
            }

            TempData["AlertMessage"] = alertMessage;

            return RedirectToAction(nameof(Week), new
            {
                branchId,
                year = deleteModel.Year,
                week = deleteModel.Week,
                department = deleteModel.Department
            });
        }

        [HttpPost]
        [Route("Copy")]
        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> CopySchedule(int branchId, DepartmentViewModel.InputCopyWeekModel copyWeekModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            TempData["AlertMessage"] = $"danger:{_localizer["MessageScheduleNotSaved"]}";

            if (ModelState.IsValid)
            {
                try
                {
                    var targetSchedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, copyWeekModel.TargetYear, copyWeekModel.TargetWeek, copyWeekModel.Department.Value);
                    var targetShifts = await _wrapper.Shift.GetAll(shift => shift.ScheduleId == targetSchedule.Id);

                    if (!targetShifts.Any())
                    {
                        var schedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, copyWeekModel.Year, copyWeekModel.Week, copyWeekModel.Department.Value);
                        var shifts = await _wrapper.Shift.GetAll(shift => shift.ScheduleId == schedule.Id);

                        var newShifts = shifts.Select(shift => new Shift
                        {
                            ScheduleId = targetSchedule.Id,
                            UserId = shift.UserId,
                            Date = ISOWeek.ToDateTime(copyWeekModel.TargetYear, copyWeekModel.TargetWeek, shift.Date.DayOfWeek),
                            StartTime = shift.StartTime,
                            EndTime = shift.EndTime
                        }).ToArray();

                        if (await _wrapper.Shift.AddRange(newShifts) != null)
                        {
                            TempData["AlertMessage"] = $"success:{_localizer["MessageScheduleCopied", copyWeekModel.TargetWeek, copyWeekModel.TargetYear]}";

                            return RedirectToAction(nameof(Week), new
                            {
                                branchId,
                                year = copyWeekModel.TargetYear,
                                week = copyWeekModel.TargetWeek,
                                department = copyWeekModel.Department
                            });
                        }
                    }
                    else
                    {
                        TempData["AlertMessage"] = $"danger:{_localizer["MessageScheduleNotEmpty"]}";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    TempData["AlertMessage"] = $"danger:{_localizer["MessageWeekNotExists"]}";
                }
            }

            return RedirectToAction(nameof(Week), new
            {
                branchId,
                year = copyWeekModel.Year,
                week = copyWeekModel.Week,
                department = copyWeekModel.Department
            });
        }

        [HttpPost]
        [Route("Approve")]
        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> ApproveSchedule(int branchId, DepartmentViewModel.InputApproveScheduleModel approveScheduleModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            TempData["AlertMessage"] = $"danger:{_localizer["MessageScheduleNotApproved"]}";

            if (ModelState.IsValid)
            {
                try
                {
                    var schedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, approveScheduleModel.Year, approveScheduleModel.Week, approveScheduleModel.Department.Value);
                    var shifts = await _wrapper.Shift.GetAll(shift => shift.ScheduleId == schedule.Id);

                    if (shifts.Any())
                    {
                        schedule.Confirmed = true;

                        if (await _wrapper.BranchSchedule.Update(schedule) != null)
                        {
                            TempData["AlertMessage"] = $"success:{_localizer["MessageScheduleApproved"]}";
                        }
                    }
                    else
                    {
                        TempData["AlertMessage"] = $"danger:{_localizer["MessageScheduleEmpty"]}";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    TempData["AlertMessage"] = $"danger:{_localizer["MessageWeekNotExists"]}";
                }
            }

            return RedirectToAction(nameof(Week), new
            {
                branchId,
                year = approveScheduleModel.Year,
                week = approveScheduleModel.Week,
                department = approveScheduleModel.Department
            });
        }


        public IActionResult Personal()
        {
            return View(new PersonalViewModel
            {
                InputOfferShift = new PersonalViewModel.InputOfferShiftModel()
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetCalendarEvents(int branchId, [FromQuery(Name = "start")] DateTime startDate, [FromQuery(Name = "end")] DateTime endDate)
        {
            var userId = int.Parse(_userManager.GetUserId(User));

            var shifts = await _wrapper.Shift.GetAll(
            shift => shift.UserId == userId,
            shift => shift.Schedule.BranchId == branchId,
            shift => shift.Schedule.Confirmed,
            shift => shift.Date >= startDate,
            shift => shift.Date <= endDate
            );

            return Json(shifts.Select(shift => new EventViewModel
            {
                Id = shift.Id,
                Title = shift.Schedule.Department.ToString(),
                Start = $"{shift.Date:yyyy-MM-dd}T{shift.StartTime}",
                End = $"{shift.Date:yyyy-MM-dd}T{shift.EndTime}",
                AllDay = false,
                ExtendedProps = new Dictionary<string, object>
                {
                    {
                        "offered", shift.Offered
                    }
                }
            }));
        }

        [HttpPost]
        public async Task<IActionResult> OfferShift(int branchId, PersonalViewModel.InputOfferShiftModel model)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            TempData["AlertMessage"] = $"danger:{_localizer["MessageShiftNotOffered"]}";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.Shift.Get(shift1 => shift1.Id == model.ShiftId);

                shift.Offered = true;

                if (await _wrapper.Shift.Update(shift) != null)
                {
                    TempData["AlertMessage"] = $"success:{_localizer["MessageShiftOffered"]}";
                }
            }

            return RedirectToAction(nameof(Personal));
        }

        public async Task<IActionResult> Offers(int branchId)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            var userId = int.Parse(_userManager.GetUserId(User));

            var departments = (await _wrapper.UserBranch.GetAll(userBranch => userBranch.UserId == userId)).Select(userBranch => userBranch.Department);

            var shifts = await _wrapper.Shift.GetAll(
            shift => shift.Offered,
            shift => shift.Schedule.BranchId == branch.Id,
            shift => departments.Contains(shift.Schedule.Department),
            shift => shift.Date >= DateTime.Today,
            shift => shift.WorkedShift == null);

            return View(new OffersViewModel
            {
                Shifts = shifts.GroupBy(shift => shift.Date)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(shift => new OffersViewModel.Shift
                    {
                        Id = shift.Id,
                        OwnedShift = shift.User.Id == userId,
                        Department = shift.Schedule.Department,
                        Employee = UserUtil.GetFullName(shift.User),
                        StartTime = shift.StartTime,
                        EndTime = shift.EndTime
                    }).ToList())
            });
        }

        [HttpPost]
        public async Task<IActionResult> AdoptOffer(int branchId, OffersViewModel model)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            var alertMessage = $"danger:{_localizer["MessageOfferNotAdopted"]}";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.Shift.Get(shift1 => shift1.Id == model.Input.ShiftId);
                var userId = int.Parse(_userManager.GetUserId(User));

                if (shift != null)
                {
                    if (shift.UserId == userId)
                    {
                        shift.Offered = false;
                    }
                    else if (shift.Offered)
                    {
                        shift.Offered = false;
                        shift.UserId = userId;
                    }

                    if (await _wrapper.Shift.Update(shift) != null)
                    {
                        alertMessage = shift.UserId == userId ? $"success:{_localizer["MessageOfferCanceled"]}" : $"success:{_localizer["MessageOfferAdopted"]}";
                    }
                }
            }

            TempData["AlertMessage"] = alertMessage;

            return RedirectToAction(nameof(Offers));
        }

        [Route("{shiftId}")]
        [HttpGet]
        public async Task<IActionResult> CreateCrossBranchOffer(int shiftId)
        {
            var shift = await _wrapper.Shift.Get(
            s => s.Id == shiftId
            );

            var vmShift = new CrossBranchViewModel.Shift
            {
                Date = shift.Date,
                EndTime = shift.EndTime,
                StartTime = shift.StartTime,
                Id = shift.Id,
                User = shift.User
            };

            return View(vmShift);
        }

        [HttpPost]
        [Route("{shiftId}")]
        public async Task<IActionResult> ConfirmCreateCrossBranchOffer(int shiftId)
        {
            var shift = await _wrapper.Shift.Get(s => s.Id == shiftId);
            shift.OfferedCrossBranch = true;
            await _wrapper.Shift.Update(shift);

            return RedirectToAction("Week");
        }

        [HttpGet]
        public async Task<IActionResult> CrossBranchOffers(int branchId)
        {
            var shifts = await ConvertShifts(await _wrapper.Shift.GetAll(
            s => s.OfferedCrossBranch,
            s => s.Schedule.BranchId != branchId// Do not list requests from own branch
            ));
            return View(shifts);
        }

        [HttpGet]
        [Route("adopt/{shiftId}")]
        public async Task<IActionResult> AdoptCrossBranchOffer(int shiftId, int branchId)
        {
            var shift = await ConvertShift(await _wrapper.Shift.Get(s => s.Id == shiftId));
            var availableEmployees = await _wrapper.User.GetFreeEmployees(branchId, shift.Date, shift.Department);

            var vm = new CrossBranchViewModel.AdoptShift
            {
                Shift = shift, AvailableEmployees = availableEmployees
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAdoptCrossBranchOffer(CrossBranchViewModel.AdoptShift viewModel)
        {
            var shift = await _wrapper.Shift.Get(s => s.Id == viewModel.ShiftId);
            var user = await _wrapper.User.Get(u => u.Id == viewModel.SelectedUserId);

            shift.User = user;
            shift.OfferedCrossBranch = false;

            await _wrapper.Shift.Update(shift);

            return RedirectToAction("CrossBranchOffers");
        }

        private Department[] GetUserDepartments(ClaimsPrincipal user, int branchId)
        {
            return User.HasClaim("Manager", branchId.ToString()) ? Enum.GetValues<Department>() : Enum.GetValues<Department>().Where(department => user.HasClaim("BranchDepartment", $"{branchId}.{department}")).ToArray();
        }

        private async Task<List<CrossBranchViewModel.Shift>> ConvertShifts(List<Shift> shifts)
        {
            var convertedShifts = new List<CrossBranchViewModel.Shift>();
            shifts.ForEach(async shift => convertedShifts.Add(await ConvertShift(shift)));
            return convertedShifts;
        }

        private async Task<CrossBranchViewModel.Shift> ConvertShift(Shift shift)
        {
            var branch = await _wrapper.Branch.Get(b => b.Id == shift.Schedule.BranchId);
            return new CrossBranchViewModel.Shift
            {
                Date = shift.Date,
                EndTime = shift.EndTime,
                StartTime = shift.StartTime,
                User = shift.User,
                Id = shift.Id,
                Department = shift.Schedule.Department,
                Branch = branch
            };
        }
    }
}
