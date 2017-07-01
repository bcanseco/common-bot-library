using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class ImgurService : IWebpageService, ISearchable<IWebpage>
    {
        /// <summary>
        ///   Constructs an <see cref="IWebpageService"/> implementation that searches Imgur.
        /// </summary>
        /// <param name="imgur">Defaults to Imgur client ID in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public ImgurService(string imgur = null)
        {
            ClientId = imgur ?? Tokens.Imgur;

            if (string.IsNullOrWhiteSpace(ClientId))
            {
                var msg = $"Imgur token is required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string ClientId { get; }

        /// <summary>
        ///   Searches imgur.com for images relevant to the given search term(s).
        /// </summary>
        /// <param name="query">The query to search for.</param>
        /// <returns>A collection of relevant images indexed on Imgur.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <seealso href="https://imgur.com/tos">TOS</seealso>
        public async Task<IEnumerable<ImgurResult>> SearchAsync(string query)
        {
            using (var client = new RestClient("https://api.imgur.com"))
            {
                var resource = $"3/gallery/search?q={query}";
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Authorization", $"Client-ID {ClientId}");

                var response = await client.ExecuteAsync(request);
                var parsedResponse = JObject.Parse(response.Content);

                if (!parsedResponse["data"].HasValues)
                    return Enumerable.Empty<ImgurResult>();

                return parsedResponse["data"]
                    .Select(d => d.ToObject<ImgurResult>())
                    .Where(r => !r.IsAlbum);
            }
        }

        async Task<IEnumerable<IWebpage>> IWebpageService.SearchAsync(string query)
            => await SearchAsync(query);

        async Task<IEnumerable<IWebpage>> ISearchable<IWebpage>.SearchAsync(string query)
            => await SearchAsync(query);
    }
}
