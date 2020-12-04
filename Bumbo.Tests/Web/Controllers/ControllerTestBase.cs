using System.Security.Claims;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Tests.Utils;
using Microsoft.AspNetCore.Identity;
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
        private UserManager<User> _userManager;

        public ApplicationDbContext Context => _context ??= TestDatabaseContextFactory.CreateDbContext();
        public RepositoryWrapper Wrapper => _wrapper ??= new RepositoryWrapper(Context);
        public IStringLocalizer<TController> Localizer => _localizer ??= CreateStringLocalizer();

        public UserManager<User> UserManager => _userManager ??= MockUserManager();

        private IStringLocalizer<TController> CreateStringLocalizer()
        {
            var options = Options.Create(new LocalizationOptions {ResourcesPath = "Resources"});
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            return new StringLocalizer<TController>(factory);
        }

        // Based on: https://stackoverflow.com/a/52562694/10557332
        private UserManager<User> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var managerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            managerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            return managerMock.Object;
        }
    }
}
