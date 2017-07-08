using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class PandorabotServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Pandorabot = null;
            var service = new PandorabotService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new PandorabotService(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Should_Fail_With_Blank_Input()
        {
            var service = new PandorabotService("?");
            var response = await service.ConverseAsync(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Should_Fail_With_Null_Input()
        {
            var service = new PandorabotService("?");
            var response = await service.ConverseAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var service = new PandorabotService("?");
            var response = await service.ConverseAsync("How are you?");
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Credentials()
        {
            // Get valid Pandorabot token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new PandorabotService();
            var response = await service.ConverseAsync("Hey, how are ya?", "abc");

            Assert.IsNotNull(response.Reply);
            Assert.AreSame(response.ConversationId, "abc");
        }

        [TestMethod]
        public async Task Should_Create_ConversationId_If_Not_Provided()
        {
            // Get valid Pandorabot token
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new PandorabotService();
            var response = await service.ConverseAsync("Hey, how are ya?");

            Assert.IsNotNull(response.Reply);
            Assert.IsNotNull(response.ConversationId);
        }
    }
}
