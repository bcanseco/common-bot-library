using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class MyAnimeListServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.MyAnimeList = null;
            var service = new MyAnimeListService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new MyAnimeListService(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new MyAnimeListService("?", "?");
            var results = await service.SearchAsync(null);
        }

        [TestMethod]
        public async Task Should_Return_Empty_When_None_Found()
        {
            // Get valid MAL username and password
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new MyAnimeListService();
            var results = await service.SearchAsync("!@#$%^&*()");

            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public async Task Should_Find_Anime_With_Real_Query()
        {
            // Get valid MAL username and password
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new MyAnimeListService();
            var results = (await service.SearchAsync("code geass")).ToList();

            Assert.IsTrue(results.Any());
            Assert.IsNotNull(results.First().Id);
        }
    }
}
