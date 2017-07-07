using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class BotLibreServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.BotLibre = null;
            var service = new BotLibreService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new BotLibreService(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Should_Fail_With_Blank_Input()
        {
            var service = new BotLibreService("?", "?");
            var response = await service.ConverseAsync(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Should_Fail_With_Null_Input()
        {
            var service = new BotLibreService("?", "?");
            var response = await service.ConverseAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new BotLibreService("1", "2");
            var response = await service.ConverseAsync("How are you?");
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Credentials()
        {
            // Get valid BotLibre tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new BotLibreService();
            var response = await service.ConverseAsync("How are you?");

            Assert.IsNotNull(response);
        }
    }
}
