using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Logic.ClockSystem;
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
        public async Task<IActionResult> Tag(int tagId)
        {
            var tag = _wrapper.ClockSystemTag.Get(myTag => myTag.SerialNumber == tagId.ToString());
            if (tag.Result == null)
            {
                return NotFound();
            }

            ClockSystemLogic rules = new ClockSystemLogic(_wrapper);

            await rules.HandleTagScan(tag.Result.User);

            return View();
        }
    }
}
