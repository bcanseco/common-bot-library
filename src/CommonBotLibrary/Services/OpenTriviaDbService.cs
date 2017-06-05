using System;
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
using Category = CommonBotLibrary.Services.Models.OpenTriviaDbCategory;
using Difficulty = CommonBotLibrary.Services.Models.OpenTriviaDbDifficulty;
using Type = CommonBotLibrary.Services.Models.OpenTriviaDbType;

namespace CommonBotLibrary.Services
{
    public class OpenTriviaDbService : ITriviaService
    {
        /// <summary>
        ///   Searches opentdb.com for trivia.
        /// </summary>
        /// <param name="amount">The amount of results to retrieve (1 to 50).</param>
        /// <param name="cat">The category of questions to retrieve.</param>
        /// <param name="diff">The difficulty of questions to retrieve.</param>
        /// <param name="type">The type of questions to retrieve.</param>
        /// <returns>A collection of relevant trivia.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="ResultNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <seealso href="https://opentdb.com/api_config.php">API info</seealso>
        public async Task<IEnumerable<OpenTriviaDbResult>> GetTriviaAsync(
            int amount = 50, Category cat = Category.Any, Difficulty? diff = null, Type? type = null)
        {
            using (var client = new RestClient("https://opentdb.com"))
            {
                var resource = $"api.php?amount={amount}&category={(int) cat}" +
                               $"&difficulty={diff.ToString().ToLower()}&type={type.ToString().ToLower()}";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsed = JObject.Parse(response.Content);

                switch (parsed["response_code"].Value<int>())
                {
                    case 0: return parsed["results"].Select(r => r.ToObject<OpenTriviaDbResult>());
                    case 1: throw new ResultNotFoundException("Not enough questions for query amount.");
                    case 2: throw new ArgumentException("One or more arguments were rejected.");
                    default: throw new NotImplementedException();
                }
            }
        }

        async Task<IEnumerable<TriviaBase>> ITriviaService.GetTriviaAsync(int amount)
            => await GetTriviaAsync(amount);
    }
}
