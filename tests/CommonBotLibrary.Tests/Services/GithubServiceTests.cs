using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class GithubServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Github = null;
            var service = new GithubService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var service = new GithubService(string.Empty, string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Should_Fail_With_Unknown_Repository()
        {
            var service = new GithubService("?", "?", "?");
            var isCreated = await service.CreateIssueAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public async Task Should_Fail_With_Invalid_OAuth()
        {
            var service = new GithubService("bcanseco", "magnanibot", "?");
            var isCreated = await service.CreateIssueAsync(null);
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Credentials()
        {
            // Get valid Github tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new GithubService();
            var isCreated = await service.CreateIssueAsync("Test", "testing", new[] {"test"});

            Assert.IsTrue(isCreated);
        }
    }
}
