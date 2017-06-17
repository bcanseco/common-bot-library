using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using RedditSharp;
using System.Linq;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Services.Models;
using RedditSharp.Things;
using Category = CommonBotLibrary.Services.Models.RedditResult.PostCategory;

namespace CommonBotLibrary.Services
{
    public class RedditService : IWebpageService, ISearchable<IWebpage>
    {
        /// <summary>
        ///   Gets relevant posts from a subreddit.
        /// </summary>
        /// <param name="subredditName">The name of the subreddit to fetch from, or the front page if null.</param>
        /// <param name="category">What category posts should be retrieved from.</param>
        /// <param name="limit">How many results should be retrieved.</param>
        /// <returns>A collection of relevant webpages indexed on a subreddit.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="ResultNotFoundException">
        ///   Thrown if a subreddit matching <paramref name="subredditName"/> is private or not found.
        /// </exception>
        /// <seealso href="https://github.com/reddit/reddit/wiki/API">TOS</seealso>
        public async Task<IEnumerable<Post>> GetPostsAsync(
            string subredditName = null, Category category = default(Category), int limit = 50)
        {
            var reddit = new Reddit();

            var subreddit = string.IsNullOrWhiteSpace(subredditName)
                ? reddit.FrontPage
                : await reddit.GetSubredditOrRAllAsync(subredditName);

            if (subreddit == null)
                throw new ResultNotFoundException($"No subreddits were found matching \"{subredditName}\".");

            switch (category)
            {
                case Category.New: return subreddit.New.Take(limit);
                case Category.Controversial: return subreddit.Controversial.Take(limit);
                case Category.Rising: return subreddit.Rising.Take(limit);
                case Category.Hot: return subreddit.Hot.Take(limit);
                case Category.Top: return subreddit.GetTop(FromTime.All).Take(limit);
                default: throw new NotImplementedException($"Unknown post category: {category}.");
            }
        }

        async Task<IEnumerable<IWebpage>> IWebpageService.SearchAsync(string subreddit)
            => (await GetPostsAsync(subreddit)).Select(p => new RedditResult(p));

        async Task<IEnumerable<IWebpage>> ISearchable<IWebpage>.SearchAsync(string subreddit)
            => (await GetPostsAsync(subreddit)).Select(p => new RedditResult(p));
    }
}
