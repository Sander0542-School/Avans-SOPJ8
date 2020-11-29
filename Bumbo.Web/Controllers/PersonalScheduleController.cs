using Bumbo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchManager")]
    [Route("Branches/{branchId}/Schedule/{controller}/{action=Index}")]
    public class PersonalScheduleController : Controller
    {

        private readonly RepositoryWrapper _wrapper;
        private readonly IStringLocalizer<PersonalScheduleController> _localizer;

        public PersonalScheduleController(RepositoryWrapper wrapper, IStringLocalizer<PersonalScheduleController> localizer)
        {
            _wrapper = wrapper;
            _localizer = localizer;
        }
       


    }
}
