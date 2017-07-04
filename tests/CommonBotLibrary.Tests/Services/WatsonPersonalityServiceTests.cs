using System;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class WatsonPersonalityServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Watson = null;
            var service = new WatsonPersonalityService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new WatsonPersonalityService(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new WatsonPersonalityService("?", "?");
            var result = await service.AnalyzeAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Should_Fail_With_Null_Text()
        {
            // Get valid Watson tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new WatsonPersonalityService();
            var result = await service.AnalyzeAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Should_Fail_With_Insufficiently_Long_String()
        {
            // Get valid Watson tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new WatsonPersonalityService();
            var text = string.Join(" ", Enumerable.Repeat("test", 99));
            var result = await service.AnalyzeAsync(text);
        }

        [TestMethod]
        public async Task Should_Work_With_Sufficiently_Long_String()
        {
            // Get valid Watson tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new WatsonPersonalityService();
            var text = string.Join(" ", Enumerable.Repeat("test", 100));
            var result = await service.AnalyzeAsync(text);

            Assert.AreEqual(100, result.WordCount);
        }
    }
}
