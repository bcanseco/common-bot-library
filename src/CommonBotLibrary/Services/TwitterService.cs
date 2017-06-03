using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace CommonBotLibrary.Services
{
    public class TwitterService : IWebpageService, ISearchable<IWebpage>
    {
        /// <summary>
        ///   Constructs an <see cref="IWebpageService"/> implementation that searches Twitter.
        /// </summary>
        /// <param name="credentials">Defaults to Twitter property in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public TwitterService(ITwitterCredentials credentials = null)
        {
            Auth.SetCredentials(credentials ?? Tokens.Twitter);

            if (!Auth.Credentials?.AreSetupForUserAuthentication() ?? true)
            {
                var msg = $"Twitter tokens are required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        /// <summary>
        ///   Searches twitter.com for recent tweets of a user.
        /// </summary>
        /// <param name="handle">The handle of the user to fetch tweets for.</param>
        /// <param name="includeRTs">Include retweets in response or not.</param>
        /// <param name="includeReplies">Include replies in response or not.</param>
        /// <returns>A collection of relevant tweets.</returns>
        /// <exception cref="AggregateException">
        ///   Thrown if a network connection cannot be made, if credentials are invalid,
        ///    or if a user for the given handle cannot be found.
        /// </exception>
        /// <exception cref="ResultNotFoundException">
        ///   Thrown if the matched account is protected.
        /// </exception>
        /// <seealso href="https://dev.twitter.com/overview/terms">TOS</seealso>
        public async Task<IEnumerable<ITweet>> GetRecentTweetsAsync(
            string handle, bool includeRTs = true, bool includeReplies = false)
        {
            var user = User.GetUserFromScreenName(handle.Replace("@", ""));

            if (user == null)
            {
                var msg = $"Either Twitter tokens are invalid or no user was found with handle \"{handle}\".";
                throw new AggregateException(
                    new ResultNotFoundException(msg), new InvalidCredentialsException(msg));
            }

            var timelineOptions = new UserTimelineParameters
            {
                IncludeContributorDetails = true,
                IncludeRTS = includeRTs,
                ExcludeReplies = includeReplies
            };

            var tweets = await TimelineAsync.GetUserTimeline(user, timelineOptions);

            if (tweets == null)
                throw new ResultNotFoundException($"Cannot get tweets; \"{handle}\"'s account is private.");

            return tweets;
        }

        async Task<IEnumerable<IWebpage>> IWebpageService.SearchAsync(string handle)
            => (await GetRecentTweetsAsync(handle)).Select(t => new TwitterResult(t));

        async Task<IEnumerable<IWebpage>> ISearchable<IWebpage>.SearchAsync(string handle)
            => (await GetRecentTweetsAsync(handle)).Select(t => new TwitterResult(t));
    }
}
