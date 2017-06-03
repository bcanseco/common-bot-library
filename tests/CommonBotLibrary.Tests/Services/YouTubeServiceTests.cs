using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class YouTubeServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Google = null;
            var service = new YouTubeService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new YouTubeService(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(Google.GoogleApiException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new YouTubeService("?");
            var videos = await service.SearchAsync(null);
        }

        [TestMethod]
        public async Task Should_Return_Empty_When_None_Found()
        {
            // Get valid Google token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new YouTubeService();
            var videos = await service.SearchAsync("1@3$mNbVuNyBtVrC");

            Assert.IsFalse(videos.Any());
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Search_Query()
        {
            // Get valid Google token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new YouTubeService();
            var videos = (await service.SearchAsync("cats")).ToList();

            Assert.IsTrue(videos.Any());
            Assert.IsNotNull(videos.First().Id);
        }
    }
}
