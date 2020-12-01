using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bumbo.Web.Controllers
{
    //[Authorize(Policy = "BranchEmployee")]
    [Route("{controller}/{action=Index}")]
    public class PersonalScheduleController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly IStringLocalizer<PersonalScheduleController> _localizer;
        private readonly UserManager<User> _userManager;

        public PersonalScheduleController(RepositoryWrapper wrapper, IStringLocalizer<PersonalScheduleController> localizer, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _localizer = localizer;
            _userManager = userManager;
        }
        
       
    }
}
