using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using SafeEnum = Google.Apis.Customsearch.v1.CseResource.ListRequest.SafeEnum;

namespace CommonBotLibrary.Services
{
    public class GoogleSearchService : IWebpageService, ISearchable<IWebpage>
    {
        /// <summary>
        ///   Constructs an <see cref="IWebpageService"/> implementation that searches Google.
        /// </summary>
        /// <param name="platformKey">Defaults to platform key in <see cref="Tokens"/> if null.</param>
        /// <param name="engineId">Defaults to engine ID in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public GoogleSearchService(string platformKey = null, string engineId = null)
        {
            PlatformKey = platformKey ?? Tokens.Google?.PlatformKey;
            EngineId = engineId ?? Tokens.Google?.EngineId;

            if (string.IsNullOrWhiteSpace(PlatformKey) || string.IsNullOrWhiteSpace(EngineId))
            {
                var msg = $"Google tokens are required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string PlatformKey { get; }
        private string EngineId { get; }

        /// <summary>
        ///   Searches google.com for webpages relevant to the given search term(s).
        /// </summary>
        /// <param name="query">The query to search for.</param>
        /// <param name="safeSearch">Safe search configuration.</param>
        /// <returns>A collection of relevant webpages indexed on Google.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="Google.GoogleApiException">
        ///   Thrown if Google is down or rejects your token/s.
        /// </exception>
        /// <seealso href="https://cse.google.com">Custom Search Engine info</seealso>
        public async Task<IEnumerable<Result>> SearchAsync(string query, SafeEnum safeSearch = SafeEnum.Off)
        {
            // Set up Google custom search service
            var googleService = new CustomsearchService(
                new BaseClientService.Initializer {ApiKey = PlatformKey});

            // Configure query information
            var searchListRequest = googleService.Cse.List(query);
            searchListRequest.Cx = EngineId;
            searchListRequest.Safe = safeSearch;
            searchListRequest.Num = 10; // max allowed by google; also the default

            var results = await searchListRequest.ExecuteAsync();

            return results.Items?.Where(i => i.Kind == "customsearch#result")
                   ?? Enumerable.Empty<Result>();
        }

        async Task<IEnumerable<IWebpage>> IWebpageService.SearchAsync(string query)
            => (await SearchAsync(query)).Select(r => new GoogleSearchResult(r));

        async Task<IEnumerable<IWebpage>> ISearchable<IWebpage>.SearchAsync(string query)
            => (await SearchAsync(query)).Select(r => new GoogleSearchResult(r));
    }
}
