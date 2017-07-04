using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class GoogleVisionServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Google = null;
            var service = new GoogleVisionService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new GoogleVisionService(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(Google.GoogleApiException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new GoogleVisionService("?");
            var result = await service.AnalyzeAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Should_Fail_With_Invalid_Image_Link()
        {
            // Get valid Google token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new GoogleVisionService();
            var result = await service.AnalyzeAsync("https://google.com");
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Image_Link()
        {
            // Get valid Google token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new GoogleVisionService();
            var result = await service.AnalyzeAsync("https://i.imgur.com/VIkoEtC.jpg");

            Assert.IsNotNull(result.TextAnnotations);
        }
    }
}
