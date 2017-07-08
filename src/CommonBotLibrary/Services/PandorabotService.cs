using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class PandorabotService : IChatService
    {
        /// <summary>
        ///   Constructs an <see cref="IChatService"/> implementation that uses Pandorabots.
        /// </summary>
        /// <param name="botId">Defaults to bot Id in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public PandorabotService(string botId = null)
        {
            BotId = botId ?? Tokens.Pandorabot;

            if (string.IsNullOrWhiteSpace(BotId))
            {
                var msg = $"Pandorabot token is required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }
        
        private string BotId { get; }

        /// <summary>
        ///   Uses pandorabots.com to communicate with an AI.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="customerId">A unique identifier for the current conversation.</param>
        /// <returns>The AI's response, and the current conversation ID.</returns>
        /// <exception cref="ArgumentNullException">
        ///   Thrown if <paramref name="message"/> is null or empty.
        /// </exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        ///   Thrown when internet issues occurr, when credentials are rejected, etc.
        /// </exception>
        /// <seealso href="https://pandorabots.com/static/html/Legal/">TOS</seealso>
        public async Task<(string Reply, string ConversationId)> ConverseAsync(string message, string customerId = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Message cannot be null or empty.");

            using (var client = new RestClient("https://www.pandorabots.com"))
            {
                var resource = $"pandora/talk-xml?input={WebUtility.UrlEncode(message)}" +
                               $"&botid={BotId}&custid={customerId}";
                var request = new RestRequest(resource, Method.POST);
                var response = await client.ExecuteAsync(request);

                var parsed = XDocument.Parse(response.Content);
                
                var reply = parsed.Element("result")?.Element("that")?.Value;
                customerId = customerId ?? parsed.Element("result")?.Attribute("custid")?.Value;

                return (reply, customerId);
            }
        }
    }
}
