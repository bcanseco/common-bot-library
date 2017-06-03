using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class OmdbServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Omdb = null;
            var service = new OmdbService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new OmdbService(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new OmdbService("?");
            var movies = await service.SearchAsync(null);
        }

        [TestMethod]
        public async Task Search_Should_Return_Empty_With_Invalid_Movie()
        {
            // Get valid OMDb key
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new OmdbService();
            var movies = await service.SearchAsync("...");

            Assert.IsFalse(movies.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Direct_Should_Fail_With_Invalid_Movie()
        {
            // Get valid OMDb key
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new OmdbService();
            var movie = await service.DirectAsync("...");
        }

        [TestMethod]
        public async Task Search_Should_Work_With_Valid_Movie()
        {
            // Get valid OMDb key
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new OmdbService();
            var movies = (await service.SearchAsync("shawshank")).ToList();

            Assert.IsTrue(movies.Any());
            Assert.IsNotNull(movies.First().ImdbId);
        }

        [TestMethod]
        public async Task Direct_Should_Work_With_Valid_Movie()
        {
            // Get valid OMDb key
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new OmdbService();
            var movie = await service.DirectAsync("batman");

            Assert.IsNotNull(movie.Title);
            Assert.IsNotNull(movie.Actors);
        }
    }
}
