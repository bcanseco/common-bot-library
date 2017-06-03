using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class MyAnimeListService : IAnimeService, IWebpageService, ISearchable<AnimeBase>
    {
        /// <summary>
        ///   Constructs an <see cref="IAnimeService"/> implementation that searches MAL.
        /// </summary>
        /// <param name="username">Defaults to username in <see cref="Tokens"/> if null.</param>
        /// <param name="password">Defaults to password in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public MyAnimeListService(string username = null, string password = null)
        {
            Username = username ?? Tokens.MyAnimeList?.Username;
            Password = password ?? Tokens.MyAnimeList?.Password;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                var msg = $"MAL tokens are required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string Username { get; }
        private string Password { get; }

        /// <summary>
        ///   Searches myanimelist.net for anime that match a given title.
        /// </summary>
        /// <param name="title">The anime title to search for.</param>
        /// <returns>A collection of relevant anime.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException">
        ///   Thrown if MAL returns a 401 for the provided username and password.
        /// </exception>
        /// <seealso href="https://myanimelist.net/modules.php?go=api">API Docs</seealso>
        public async Task<IEnumerable<MyAnimeListResult>> SearchAsync(string title)
        {
            using (var client = new RestClient("https://myanimelist.net"))
            {
                client.Authenticator = new HttpBasicAuthenticator(Username, Password);

                var resource = $"api/anime/search.xml?q={title}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                if (response.StatusCode == HttpStatusCode.NoContent)
                    return Enumerable.Empty<MyAnimeListResult>();

                var parsedResponse = XDocument.Parse(response.Content);

                return parsedResponse.Root?.Elements("entry")
                    .Select(e => new MyAnimeListResult(e));
            }
        }

        async Task<IEnumerable<AnimeBase>> IAnimeService.SearchAsync(string title)
            => await SearchAsync(title);

        async Task<IEnumerable<IWebpage>> IWebpageService.SearchAsync(string title)
            => await SearchAsync(title);

        async Task<IEnumerable<AnimeBase>> ISearchable<AnimeBase>.SearchAsync(string title)
            => await SearchAsync(title);
    }
}
