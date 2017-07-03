using System;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class YandexTranslateServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Yandex = null;
            var service = new YandexTranslateService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new YandexTranslateService(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new YandexTranslateService("?");
            var result = await service.TranslateAsync("test", "es");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Should_Fail_With_Blank_Params()
        {
            var service = new YandexTranslateService("?");
            var result = await service.TranslateAsync(null, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Should_Fail_With_Unsupported_Language()
        {
            var service = new YandexTranslateService("?");
            var result = await service.TranslateAsync("test", "Pirate");
        }

        [TestMethod]
        public async Task Should_Work_Without_Source()
        {
            // Get valid Yandex token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new YandexTranslateService();
            var result = await service.TranslateAsync("Hello", "Spanish");

            Assert.AreEqual("Hola", result.OutputText);
        }

        [TestMethod]
        public async Task Should_Work_With_Source()
        {
            // Get valid Yandex token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new YandexTranslateService();
            var result = await service.TranslateAsync("Hello", "es", "English");

            Assert.AreEqual("Hola", result.OutputText);
        }
    }
}
