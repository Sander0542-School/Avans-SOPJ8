using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.ClockSystem;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bumbo.Web.Controllers
{
    [Route("Api/{controller}/{action}")]
    public class ClockSystemController : Controller
    {
        private readonly ILogger<WorkedShiftController> _logger;
        private readonly RepositoryWrapper _wrapper;

        public ClockSystemController(ILogger<WorkedShiftController> logger, RepositoryWrapper wrapper)
        {
            _logger = logger;
            _wrapper = wrapper;
        }

        [HttpPost]
        public IActionResult Tag(int tagId)
        {
            var getTag = _wrapper.ClockSystemTag.Get(myTag => myTag.SerialNumber == tagId.ToString());
            if (getTag.Result == null)
            {
                return NotFound();
            }


            return View();
        }
    }
}
