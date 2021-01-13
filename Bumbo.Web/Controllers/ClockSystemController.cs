using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Logic.ClockSystem;
using Bumbo.Logic.Utils;
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
            var tag = await _wrapper.ClockSystemTag.Get(myTag => myTag.SerialNumber == tagId.ToString());
            if (tag == null)
            {
                return NotFound();
            }

            var rules = new ClockSystemLogic(_wrapper);

            await rules.HandleTagScan(tag.User);

            return View("Index", UserUtil.GetFullName(tag.User));
        }
    }
}
