using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class GoogleServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Google = null;
            var service = new GoogleService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new GoogleService(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(Google.GoogleApiException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new GoogleService("?", "?");
            var results = await service.SearchAsync(null);
        }

        [TestMethod]
        public async Task Should_Return_Empty_When_None_Found()
        {
            // Get valid Google tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new GoogleService();
            var results = await service.SearchAsync("1@3$mNbVuNyBtVrC");

            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public async Task Should_Find_Webpages_With_Real_Query()
        {
            // Get valid Google tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new GoogleService();
            var results = (await service.SearchAsync("elephants")).ToList();

            Assert.IsTrue(results.Any());
            Assert.IsNotNull(results.First().Title);
        }
    }
}
