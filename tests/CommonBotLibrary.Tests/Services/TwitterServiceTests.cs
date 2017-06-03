using System;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Models;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class TwitterServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_Without_Credentials()
        {
            Tokens.Twitter = null;
            var service = new TwitterService();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCredentialsException))]
        public void Should_Fail_With_Blank_Credentials()
        {
            var credentials = new TwitterCredentials("", "", "", "");
            var service = new TwitterService(credentials);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task Should_Fail_With_Invalid_Credentials()
        {
            var credentials = new TwitterCredentials("?", "?", "?", "?");
            var service = new TwitterService(credentials);
            var tweets = await service.GetRecentTweetsAsync("@dril");
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task Should_Throw_Exception_When_Handle_Not_Found()
        {
            // Get valid Twitter tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new TwitterService();
            var tweets = await service.GetRecentTweetsAsync(".");
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Handle()
        {
            // Get valid Twitter tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new TwitterService();
            var tweets = await service.GetRecentTweetsAsync("@cia");
            Assert.IsNotNull(tweets.First().FullText);
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Should_Fail_With_Private_Account()
        {
            // Get valid Twitter tokens
            await Tokens.LoadAsync("../../../../../tokens.json");

            var service = new TwitterService();
            var tweets = await service.GetRecentTweetsAsync("privateaccount");
        }
    }
}
