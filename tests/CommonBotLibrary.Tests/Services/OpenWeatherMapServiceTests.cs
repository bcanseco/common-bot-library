using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class OpenWeatherMapServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.OpenWeatherMap = null;
            var service = new OpenWeatherMapService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new OpenWeatherMapService(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new OpenWeatherMapService("?");
            var weather = await service.GetCurrentWeatherAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Get_Weather_Should_Fail_With_Invalid_Location()
        {
            // Get valid OpenWeatherMap API key
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new OpenWeatherMapService();
            var weather = await service.GetCurrentWeatherAsync("././");
        }

        [TestMethod]
        public async Task Get_Weather_Should_Work_With_Valid_Location()
        {
            // Get valid OpenWeatherMap API key
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new OpenWeatherMapService();
            var weather = await service.GetCurrentWeatherAsync("miami");

            Assert.IsNotNull(weather.Name);
            Assert.IsTrue(weather.Temperature > 0);
        }
    }
}
