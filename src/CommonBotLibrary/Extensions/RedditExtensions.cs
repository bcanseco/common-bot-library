using System.Threading.Tasks;
using RedditSharp;
using RedditSharp.Things;

namespace CommonBotLibrary.Extensions
{
    public static class RedditExtensions
    {
        /// <summary>
        ///   Gets a subreddit or /r/all by name.
        /// </summary>
        /// <remarks>RedditSharp doesn't treat /r/all as a subreddit.</remarks>
        public static async Task<Subreddit> GetSubredditOrRAllAsync(
            this Reddit reddit, string subreddit)
        {
            if (subreddit == "all" || subreddit.Contains("/all"))
            {
                return reddit.RSlashAll;
            }
            return await reddit.GetSubredditAsync(subreddit);
        }
    }
}
