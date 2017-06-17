using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3.Data;
using YouTubeAPI = Google.Apis.YouTube.v3.YouTubeService;
using SafeSearchEnum = Google.Apis.YouTube.v3.SearchResource.ListRequest.SafeSearchEnum;

namespace CommonBotLibrary.Services
{
    public class YouTubeService : IWebpageService, ISearchable<IWebpage>
    {
        /// <summary>
        ///   Constructs an <see cref="IWebpageService"/> implementation that searches Youtube.
        /// </summary>
        /// <param name="platformKey">Defaults to platform key in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public YouTubeService(string platformKey = null)
        {
            PlatformKey = platformKey ?? Tokens.Google?.PlatformKey;

            if (string.IsNullOrWhiteSpace(PlatformKey))
            {
                var msg = $"Google tokens are required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string PlatformKey { get; }

        /// <summary>
        ///   Searches youtube.com for videos relevant to the given search term(s).
        /// </summary>
        /// <param name="query">The query to search for.</param>
        /// <param name="safeSearch">Safe search configuration.</param>
        /// <returns>A collection of relevant videos indexed on Youtube.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="Google.GoogleApiException">
        ///   Thrown if Youtube is down or rejects your token/s.
        /// </exception>
        /// <seealso href="https://developers.google.com/youtube/terms/api-services-terms-of-service">
        ///   API TOS
        /// </seealso>
        public async Task<IEnumerable<SearchResult>> SearchAsync(string query, SafeSearchEnum safeSearch = SafeSearchEnum.None)
        {
            // Set up YouTube helper service
            var youtubeService = new YouTubeAPI(
                new BaseClientService.Initializer { ApiKey = PlatformKey });

            // Configure query information
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = query;
            searchListRequest.SafeSearch = safeSearch;
            searchListRequest.MaxResults = 50; // default is 5; 50 is the maximum allowed

            var results = await searchListRequest.ExecuteAsync();

            return results.Items.Where(i => i.Id.Kind == "youtube#video");
        }

        async Task<IEnumerable<IWebpage>> IWebpageService.SearchAsync(string query)
            => (await SearchAsync(query)).Select(i => new YouTubeResult(i));

        async Task<IEnumerable<IWebpage>> ISearchable<IWebpage>.SearchAsync(string query)
            => (await SearchAsync(query)).Select(i => new YouTubeResult(i));
    }
}
