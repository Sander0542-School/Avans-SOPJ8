using System;
using System.Collections.Generic;
using System.Globalization;
using Bumbo.Data.Models;
using Bumbo.Logic.EmployeeRules;
using NUnit.Framework;

namespace Bumbo.Tests.Logic.EmployeeRules
{
    public class WorkingHoursTests
    {
        [Test]
        public void TestUserProperties_Right()
        {
            var user = new User
            {
                Shifts = new List<Shift>(),
                UserAvailabilities = new List<UserAvailability>(),
                UserAdditionalWorks = new List<UserAdditionalWork>(),
            };

            WorkingHours.ValidateUserProperties(user);

            Assert.Pass();
        }

        [Test]
        public void TestUserProperties_MissingShifts()
        {
            var user = new User
            {
                UserAvailabilities = new List<UserAvailability>(),
                UserAdditionalWorks = new List<UserAdditionalWork>(),
            };

            Assert.Catch<ArgumentException>(() => WorkingHours.ValidateUserProperties(user));
        }

        [Test]
        public void TestUserProperties_MissingAvailabilities()
        {
            var user = new User
            {
                Shifts = new List<Shift>(),
                UserAdditionalWorks = new List<UserAdditionalWork>(),
            };

            Assert.Catch<ArgumentException>(() => WorkingHours.ValidateUserProperties(user));
        }

        [Test]
        public void TestUserProperties_MissingAdditionalWorks()
        {
            var user = new User
            {
                Shifts = new List<Shift>(),
                UserAvailabilities = new List<UserAvailability>(),
            };

            Assert.Catch<ArgumentException>(() => WorkingHours.ValidateUserProperties(user));
        }

        [Test]
        public void TestShiftBetweenAvailableTime_Right()
        {
            var shift = new Shift
            {
                Date = new DateTime(2020, 1, 1),
                StartTime = new TimeSpan(16, 0, 0),
                EndTime = new TimeSpan(18, 0, 0)
            };

            var availability = new UserAvailability
            {
                Day = shift.Date.DayOfWeek,
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(19, 0, 0)
            };

            Assert.True(WorkingHours.ShiftBetweenAvailableTime(shift, availability));
        }

        [Test]
        public void TestShiftBetweenAvailableTime_TooEarly()
        {
            var shift = new Shift
            {
                Date = new DateTime(2020, 1, 1),
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(18, 0, 0)
            };

            var availability = new UserAvailability
            {
                Day = shift.Date.DayOfWeek,
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(19, 0, 0)
            };

            Assert.False(WorkingHours.ShiftBetweenAvailableTime(shift, availability));
        }

        [Test]
        public void TestShiftBetweenAvailableTime_TooLate()
        {
            var shift = new Shift
            {
                Date = new DateTime(2020, 1, 1),
                StartTime = new TimeSpan(16, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            var availability = new UserAvailability
            {
                Day = shift.Date.DayOfWeek,
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(19, 0, 0)
            };

            Assert.False(WorkingHours.ShiftBetweenAvailableTime(shift, availability));
        }

        [Test]
        public void TestShiftBetweenAvailableTime_BeginSame()
        {
            var shift = new Shift
            {
                Date = new DateTime(2020, 1, 1),
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(18, 0, 0)
            };

            var availability = new UserAvailability
            {
                Day = shift.Date.DayOfWeek,
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(19, 0, 0)
            };

            Assert.True(WorkingHours.ShiftBetweenAvailableTime(shift, availability));
        }

        [Test]
        public void TestShiftBetweenAvailableTime_EndSame()
        {
            var shift = new Shift
            {
                Date = new DateTime(2020, 1, 1),
                StartTime = new TimeSpan(16, 0, 0),
                EndTime = new TimeSpan(19, 0, 0)
            };

            var availability = new UserAvailability
            {
                Day = shift.Date.DayOfWeek,
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(19, 0, 0)
            };

            Assert.True(WorkingHours.ShiftBetweenAvailableTime(shift, availability));
        }

        [Test]
        public void TestShiftBetweenAvailableTime_Outside()
        {
            var shift = new Shift
            {
                Date = new DateTime(2020, 1, 1),
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(20, 0, 0)
            };

            var availability = new UserAvailability
            {
                Day = shift.Date.DayOfWeek,
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(19, 0, 0)
            };

            Assert.False(WorkingHours.ShiftBetweenAvailableTime(shift, availability));
        }

        [Test]
        public void TestMaxHoursPerWeek_Age15_School_Right()
        {
            var year = 2020;
            var week = 6;

            var dateTime = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

            var user = new User
            {
                Birthday = dateTime.AddYears(-15).AddMonths(-3)
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerWeek(user, year, week), new TimeSpan(40, 0, 0)); //TODO Check for School week: return 12;
        }

        [Test]
        public void TestMaxHoursPerWeek_Age15_NoSchool_Right()
        {
            var year = 2020;
            var week = 1;

            var dateTime = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

            var user = new User
            {
                Birthday = dateTime.AddYears(-15).AddMonths(-3)
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerWeek(user, year, week), new TimeSpan(40, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerWeek_Age17_Right()
        {
            var year = 2020;
            var week = 6;

            var dateTime = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

            var user = new User
            {
                Birthday = dateTime.AddYears(-17).AddMonths(-3)
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerWeek(user, year, week), new TimeSpan(40, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerWeek_Age30_Right()
        {
            var year = 2020;
            var week = 6;

            var dateTime = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

            var user = new User
            {
                Birthday = dateTime.AddYears(-30).AddMonths(-3)
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerWeek(user, year, week), new TimeSpan(60, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerDay_Age15_ExtraWork_Right()
        {
            var day = DayOfWeek.Monday;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-15).AddMonths(-3),
                UserAdditionalWorks = new List<UserAdditionalWork>
                {
                    new UserAdditionalWork
                    {
                        Day = day,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(12, 0, 0),
                    }
                }
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerDay(user, day), new TimeSpan(5, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerDay_Age15_NoExtraWork_Right()
        {
            var day = DayOfWeek.Monday;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-15).AddMonths(-3),
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerDay(user, day), new TimeSpan(8, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerDay_Age17_ExtraWork_Right()
        {
            var day = DayOfWeek.Monday;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-17).AddMonths(-3),
                UserAdditionalWorks = new List<UserAdditionalWork>
                {
                    new UserAdditionalWork
                    {
                        Day = day,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(14, 0, 0),
                    }
                }
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerDay(user, day), new TimeSpan(4, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerDay_Age17_NoExtraWork_Right()
        {
            var day = DayOfWeek.Monday;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-17).AddMonths(-3),
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerDay(user, day), new TimeSpan(9, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerDay_Age30_ExtraWork_Right()
        {
            var day = DayOfWeek.Monday;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
                UserAdditionalWorks = new List<UserAdditionalWork>
                {
                    new UserAdditionalWork
                    {
                        Day = day,
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(16, 0, 0),
                    }
                }
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerDay(user, day), new TimeSpan(12, 0, 0));
        }

        [Test]
        public void TestMaxHoursPerDay_Age30_NoExtraWork_Right()
        {
            var day = DayOfWeek.Monday;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            Assert.AreEqual(WorkingHours.MaxHoursPerDay(user, day), new TimeSpan(12, 0, 0));
        }

        [Test]
        public void TestMaxDayPerAWeek_Age15()
        {
            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-15).AddMonths(-3),
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            Assert.AreEqual(WorkingHours.MaxDayPerAWeek(user), 5);
        }

        [Test]
        public void TestMaxDayPerAWeek_Age17()
        {
            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-17).AddMonths(-3),
            };

            Assert.AreEqual(WorkingHours.MaxDayPerAWeek(user), 7);
        }

        [Test]
        public void TestMaxDayPerAWeek_Age30()
        {
            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
            };

            Assert.AreEqual(WorkingHours.MaxDayPerAWeek(user), 7);
        }

        [Test]
        public void TestValidateShift_DontWantToWork_Day()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>(),
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(16, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }

        [Test]
        public void TestValidateShift_DontWantToWork_Time_TooEarly()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(18, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }

        [Test]
        public void TestValidateShift_DontWantToWork_Time_TooLate()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(18, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(16, 0, 0),
                EndTime = new TimeSpan(19, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }

        [Test]
        public void TestValidateShift_DontWantToWork_Time_Outside()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(18, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(19, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }

        [Test]
        public void TestValidateShift_Age15_Right()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-15).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(19, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(16, 0, 0),
                EndTime = new TimeSpan(19, 0, 0),
            };

            Assert.IsEmpty(WorkingHours.ValidateShift(user, shift));
        }

        [Test]
        public void TestValidateShift_Age15_TooManyHours()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-15).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(19, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }

        [Test]
        public void TestValidateShift_Age15_TooLate()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-15).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(21, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(18, 0, 0),
                EndTime = new TimeSpan(20, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }

        [Test]
        public void TestValidateShift_Age15_TooLateAndTooManyHours()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-15).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(21, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(20, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(2));
        }

        [Test]
        public void TestValidateShift_Age17_Right()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-17).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(19, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(16, 0, 0),
                EndTime = new TimeSpan(19, 0, 0),
            };

            Assert.IsEmpty(WorkingHours.ValidateShift(user, shift));
        }

        [Test]
        public void TestValidateShift_Age17_TooManyHours()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-17).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(21, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(21, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }

        [Test]
        public void TestValidateShift_Age30_Right()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-30).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(7, 0, 0),
                        EndTime = new TimeSpan(23, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(12, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
            };

            Assert.IsEmpty(WorkingHours.ValidateShift(user, shift));
        }

        [Test]
        public void TestValidateShift_Age30_TooManyHours()
        {
            var date = DateTime.Today;

            var user = new User
            {
                Birthday = DateTime.Today.AddYears(-17).AddMonths(-3),
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = date.DayOfWeek,
                        StartTime = new TimeSpan(7, 0, 0),
                        EndTime = new TimeSpan(23, 0, 0),
                    }
                },
                UserAdditionalWorks = new List<UserAdditionalWork>()
            };

            var shift = new Shift
            {
                Date = date,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(22, 0, 0),
            };

            Assert.That(WorkingHours.ValidateShift(user, shift), Has.Count.EqualTo(1));
        }
    }
}