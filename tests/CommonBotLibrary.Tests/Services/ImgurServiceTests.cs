using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class ImgurServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Imgur = null;
            var service = new ImgurService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new ImgurService(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new ImgurService("?");
            var results = await service.SearchAsync(null);
        }

        [TestMethod]
        public async Task Should_Return_Empty_When_None_Found()
        {
            // Get valid Imgur token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new ImgurService();
            var results = await service.SearchAsync("1@3$mNbVuNyBtVrC");

            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public async Task Should_Find_Images_With_Real_Query()
        {
            // Get valid Imgur token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new ImgurService();
            var results = (await service.SearchAsync("dogs")).ToList();

            Assert.IsTrue(results.Any());
            Assert.IsNotNull(results.First().Title);
        }
    }
}
