using System;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class WatsonPersonalityService : ITextAnalysisService
    {
        /// <summary>
        ///   Constructs an <see cref="ITextAnalysisService"/> implementation that uses IBM Watson.
        /// </summary>
        /// <param name="username">Defaults to username in <see cref="Tokens"/> if null.</param>
        /// <param name="password">Defaults to password in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public WatsonPersonalityService(string username = null, string password = null)
        {
            Username = username ?? Tokens.Watson?.Username;
            Password = password ?? Tokens.Watson?.Password;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                var msg = $"Watson tokens are required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string Username { get; }
        private string Password { get; }

        /// <summary>
        ///   Uses IBM Watson machine learning to analyze text.
        /// </summary>
        /// <param name="text">The string of text to analyze.</param>
        /// <returns>Analysis results.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">
        ///   Thrown if <paramref name="text"/> has less than 100 words (Watson requirement).
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <seealso href="https://www-03.ibm.com/software/sla/sladb.nsf/sla/bm-6838-03">TOS</seealso>
        public async Task<WatsonPersonalityResult> AnalyzeAsync(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text), "Text cannot be null.");

            if (text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length < 100)
                throw new ArgumentException("Text must have at least 100 words", text);

            using (var client = new RestClient("https://gateway.watsonplatform.net"))
            {
                client.Authenticator = new HttpBasicAuthenticator(Username, Password);

                const string resource = "personality-insights/api/v3/profile?version=2016-10-20" +
                                        "&raw_scores=true&consumption_preferences=true";

                var request = new RestRequest(resource, Method.POST)
                    .AddOrUpdateHeader("Content-Type", "text/plain")
                    .AddBody(text);

                var response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<WatsonPersonalityResult>(response.Content);
            }
        }

        async Task<TextAnalysisBase> ITextAnalysisService.AnalyzeAsync(string text)
            => await AnalyzeAsync(text);
    }
}
