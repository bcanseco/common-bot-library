using System.Linq;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class RedditServiceTests
    {
        private RedditService Service { get; set; }

        [TestInitialize]
        public void InitService()
        {
            Service = new RedditService();
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Should_Throw_Exception_When_No_Subreddit_Found()
        {
            var posts = await Service.GetPostsAsync("/r/1@3$mNbVuNyBtVrC");
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Subreddit()
        {
            var posts = (await Service.GetPostsAsync("r/dataisbeautiful")).ToList();
            Assert.IsNotNull(posts.First().Title);

            var noPosts = (await Service.GetPostsAsync("/empty")).ToList();
            Assert.IsFalse(noPosts.Any());
        }

        [TestMethod]
        public async Task Should_Work_With_RAll()
        {
            var post = (await Service.GetPostsAsync("all")).First();
            Assert.IsNotNull(post.Url);
        }

        [TestMethod]
        public async Task Should_Work_With_Front_Page()
        {
            var post = (await Service.GetPostsAsync()).First();
            Assert.IsNotNull(post.Title);
        }
    }
}
