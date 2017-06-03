using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using Type = CommonBotLibrary.Services.Models.OmdbType;

namespace CommonBotLibrary.Services
{
    public class OmdbService : IMovieService, ISearchable<MovieBase>
    {
        /// <summary>
        ///   Constructs an <see cref="IMovieService"/> implementation that searches OMDb.
        /// </summary>
        /// <param name="apiKey">Defaults to API key in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public OmdbService(string apiKey = null)
        {
            ApiKey = apiKey ?? Tokens.Omdb;

            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                var msg = $"OMDb key is required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string ApiKey { get; }

        /// <summary>
        ///   Searches omdbapi.com for media that match a given title.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <param name="type">The type of content to search for.</param>
        /// <param name="year">The year the media was released.</param>
        /// <param name="plot">The length of the returned plot summary; "short" or "full"</param>
        /// <returns>A collection of relevant media filled with basic info.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <seealso href="http://www.omdbapi.com/legal.htm">API TOS</seealso>
        public async Task<IEnumerable<OmdbSearchResult>> SearchAsync(
            string title, Type type = Type.Movie, int? year = null, string plot = "short")
        {
            using (var client = new RestClient("https://omdbapi.com"))
            {
                var resource = $"?s={title}&type={type}&y={year}&plot={plot}&apiKey={ApiKey}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsed = JObject.Parse(response.Content);

                if (parsed["Response"].ToString() != "True")
                    return Enumerable.Empty<OmdbSearchResult>();

                return parsed["Search"]
                    .Select(m => m.ToObject<OmdbSearchResult>());
            }
        }

        /// <summary>
        ///   Retrieves a direct result from omdbapi.com with complete information.
        /// </summary>
        /// <param name="title">The title to use.</param>
        /// <param name="type">The type of content to search for.</param>
        /// <param name="year">The year the media was released.</param>
        /// <returns>The most relevant media for the given title.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <exception cref="ResultNotFoundException"></exception>
        /// <seealso href="http://www.omdbapi.com/legal.htm">API TOS</seealso>
        public async Task<OmdbDirectResult> DirectAsync(
            string title, Type type = Type.Movie, int? year = null)
        {
            using (var client = new RestClient("https://omdbapi.com"))
            {
                var resource = $"?t={title}&type={type}&y={year}&apiKey={ApiKey}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsed = JObject.Parse(response.Content);

                if (parsed["Response"].ToString() != "True")
                    throw new ResultNotFoundException($"No movie was found matching `{title}`.");

                return JsonConvert.DeserializeObject<OmdbDirectResult>(response.Content);
            }
        }

        async Task<IEnumerable<MovieBase>> IMovieService.SearchAsync(string title)
            => await SearchAsync(title);

        async Task<IEnumerable<MovieBase>> ISearchable<MovieBase>.SearchAsync(string title)
            => await SearchAsync(title);

        async Task<MovieBase> IMovieService.DirectAsync(string title)
            => await DirectAsync(title);
    }
}
