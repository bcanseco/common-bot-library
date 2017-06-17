using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class UrbanDictionaryService : IDictionaryService, ISearchable<DefinitionBase>
    {
        /// <summary>
        ///   Searches urbandictionary.com for definitions that match a given phrase.
        /// </summary>
        /// <param name="phrase">The phrase to search for.</param>
        /// <returns>A collection of relevant definitions.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <seealso href="http://about.urbandictionary.com/tos">TOS</seealso>
        public async Task<IEnumerable<UrbanDictionaryResult>> GetDefinitionsAsync(string phrase)
        {
            using (var client = new RestClient("https://api.urbandictionary.com"))
            {
                var resource = $"v0/define?term={phrase}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsedResponse = JObject.Parse(response.Content);

                if (parsedResponse["result_type"].ToString() == "no_results")
                    return Enumerable.Empty<UrbanDictionaryResult>();

                return parsedResponse["list"]
                    .Select(d => d.ToObject<UrbanDictionaryResult>());
            }
        }

        async Task<IEnumerable<DefinitionBase>> IDictionaryService.GetDefinitionsAsync(string phrase)
            => await GetDefinitionsAsync(phrase);

        async Task<IEnumerable<DefinitionBase>> ISearchable<DefinitionBase>.SearchAsync(string phrase)
            => await GetDefinitionsAsync(phrase);
    }
}
