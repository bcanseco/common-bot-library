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

namespace CommonBotLibrary.Services
{
    public class SteamService : IWebpageService, ISearchable<IWebpage>
    {
        /// <summary>
        ///   Searches steampowered.com for games that match a given title.
        /// </summary>
        /// <param name="title">The game title to search for.</param>
        /// <param name="currency">The currency to retrieve prices in.</param>
        /// <returns>A collection of relevant games.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<IEnumerable<SteamResult>> SearchAsync(string title, string currency = "USD")
        {
            using (var client = new RestClient("https://store.steampowered.com"))
            {
                var resource = $"api/storesearch/?term={title}&cc={currency}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                if (JObject.Parse(response.Content)["total"].Value<uint>() == 0)
                    return Enumerable.Empty<SteamResult>();

                return JObject.Parse(response.Content)["items"]
                    .Select(i => i.ToObject<SteamResult>());
            }
        }

        /// <summary>
        ///   Searches steamspy.com for statistics about a game.
        /// </summary>
        /// <param name="appId">The game id to fetch stats for.</param>
        /// <returns>Owner and player data for the matched game.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="ResultNotFoundException"></exception>
        /// <seealso href="https://steamspy.com/api.php">TOS</seealso>
        public async Task<SteamSpyData> GetSteamSpyDataAsync(uint appId)
        {
            using (var client = new RestClient("https://steamspy.com"))
            {
                var resource = $"api.php?request=appdetails&appid={appId}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var game =  JsonConvert.DeserializeObject<SteamSpyData>(response.Content);

                if (game.Name == null)
                    throw new ResultNotFoundException($"No game was found for id: {appId}.");

                return game;
            }
        }

        /// <summary>
        ///   Gets the number of current players for an app from the Steam Web API.
        /// </summary>
        /// <param name="appId">The ID of the app to retrieve players for.</param>
        /// <returns>The number of current players.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="ResultNotFoundException"></exception>
        public async Task<uint> GetCurrentPlayersAsync(uint appId)
        {
            using (var client = new RestClient("https://api.steampowered.com"))
            {
                var resource = $"ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={appId}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsedResponse = JObject.Parse(response.Content);

                if (parsedResponse["response"]["result"].Value<int>() != 1)
                    throw new ResultNotFoundException($"No player data was found for id: {appId}.");

                return parsedResponse["response"]["player_count"].Value<uint>();
            }
        }

        async Task<IEnumerable<IWebpage>> IWebpageService.SearchAsync(string title)
            => await SearchAsync(title);

        async Task<IEnumerable<IWebpage>> ISearchable<IWebpage>.SearchAsync(string title)
            => await SearchAsync(title);
    }
}
