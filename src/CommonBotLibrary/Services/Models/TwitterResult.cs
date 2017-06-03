using System.Diagnostics;
using CommonBotLibrary.Interfaces.Models;
using Tweetinvi.Models;
using Tweet = Tweetinvi.Logic.Tweet;

namespace CommonBotLibrary.Services.Models
{
    [DebuggerDisplay("{Url, nq}")]
    public class TwitterResult : IWebpage
    {
        public TwitterResult(ITweet tweet)
        {
            Url = tweet.Url;
            Title = tweet.Text;

            Tweet = tweet;
        }

        /// <summary>
        ///   The wrapped model returned by TweetInvi with more tweet props.
        /// </summary>
        public ITweet Tweet { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        public static implicit operator Tweet(TwitterResult result)
            => (Tweet) result.Tweet;
    }
}
