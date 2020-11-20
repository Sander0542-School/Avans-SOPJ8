using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bumbo.Tests.Integration.Helpers;
using Bumbo.Web;
using NUnit.Framework;

namespace Bumbo.Tests.Integration
{
    public class HomePage
    {

        private ApplicationFactory<Startup> _factory;
        private HttpClient _client;

        public HomePage()
        {
            _factory = new ApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task HomePageShouldReturnOk()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/");
            var content = defaultPage.Content;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, defaultPage.StatusCode);
        }

        [Test]
        public async Task HomePageShouldHaveLoginLink()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            var navItems= content.QuerySelectorAll("body > header > nav a");

            var foundLogin = navItems.Cast<IHtmlAnchorElement>().Any(navItem => navItem.Text.ToLower().Contains("login"));

            Assert.True(foundLogin);
        }
    }
}