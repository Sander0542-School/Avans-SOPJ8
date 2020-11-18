using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
    }
}