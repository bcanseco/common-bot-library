using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class BotLibreService : IChatService
    {
        /// <summary>
        ///   Constructs an <see cref="IChatService"/> implementation that uses BotLibre.
        /// </summary>
        /// <param name="applicationId">Defaults to username in <see cref="Tokens"/> if null.</param>
        /// <param name="botId">Defaults to repository in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public BotLibreService(string applicationId = null, string botId = null)
        {
            ApplicationId = applicationId ?? Tokens.BotLibre?.ApplicationId;
            BotId = botId ?? Tokens.BotLibre?.BotId;

            if (string.IsNullOrWhiteSpace(ApplicationId) || string.IsNullOrWhiteSpace(BotId))
            {
                var msg = $"BotLibre tokens are required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string ApplicationId { get; }
        private string BotId { get; }

        /// <summary>
        ///   Uses botlibre.com to communicate with an AI.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>The AI's response.</returns>
        /// <exception cref="ArgumentNullException">
        ///   Thrown if <paramref name="message"/> is null or empty.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        ///   Thrown when internet issues occurr, when credentials are rejected, etc.
        /// </exception>
        /// <seealso href="https://www.botlibre.com/terms.jsp">TOS</seealso>
        public async Task<string> ConverseAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Message cannot be null or empty.");

            using (var client = new RestClient("http://www.botlibre.com"))
            {
                var resource = $"rest/botlibre/form-chat?message={message}" +
                               $"&application={ApplicationId}&instance={BotId}&";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsed = XDocument.Parse(response.Content);

                return parsed.Element("response")?.Element("message")?.Value;
            }
        }
    }
}
