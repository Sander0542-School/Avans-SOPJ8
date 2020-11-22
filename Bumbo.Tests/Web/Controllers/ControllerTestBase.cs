using Bumbo.Data;
using Bumbo.Tests.Utils;
using Bumbo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Bumbo.Tests.Web.Controllers
{
    public class ControllerTestBase<TController> where TController : Controller
    {
        private ApplicationDbContext _context;
        private RepositoryWrapper _wrapper;
        private IStringLocalizer<TController> _localizer;

        public ApplicationDbContext Context => _context ??= TestDatabaseContextFactory.CreateDbContext();
        public RepositoryWrapper Wrapper => _wrapper ??= new RepositoryWrapper(Context);
        public IStringLocalizer<TController> Localizer => _localizer ??= CreateStringLocalizer();

        private IStringLocalizer<TController> CreateStringLocalizer()
        {
            var options = Options.Create(new LocalizationOptions {ResourcesPath = "Resources"});
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            return new StringLocalizer<TController>(factory);
        }
    }
}