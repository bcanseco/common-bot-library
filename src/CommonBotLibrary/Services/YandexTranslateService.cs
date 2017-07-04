using System;
using System.Net.Http;
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
    public class YandexTranslateService : ITranslationService
    {
        /// <summary>
        ///   Constructs an <see cref="ITranslationService"/> implementation that uses Yandex Translate.
        /// </summary>
        /// <param name="apiKey">Defaults to API key in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public YandexTranslateService(string apiKey = null)
        {
            ApiKey = apiKey ?? Tokens.Yandex;

            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                var msg = $"Yandex token is required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string ApiKey { get; }

        /// <summary>
        ///   Uses Yandex Translate to translate text to another language.
        /// </summary>
        /// <param name="text">The text to translate.</param>
        /// <param name="target">The language to translate to.</param>
        /// <param name="source">The language to translate from. Autodetected if null.</param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown if <paramref name="text"/> is null or empty, or if <paramref name="target"/> is null.
        /// </exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <seealso href="https://yandex.com/legal/translate_api/">TOS</seealso>
        public async Task<YandexResult> TranslateAsync(string text, YandexLanguage target, YandexLanguage source = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null.");
            if (target == null)
                throw new ArgumentNullException(nameof(target), "Target language cannot be null.");

            var languageParam = source == null
                ? target.Code
                : $"{source.Code}-{target.Code}";

            using (var client = new RestClient("https://translate.yandex.net"))
            {
                var resource = $"api/v1.5/tr.json/translate?key={ApiKey}" +
                               $"&text={text}&lang={languageParam}&options=1";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsed = JObject.Parse(response.Content);

                if (!response.IsSuccess)
                    throw new HttpRequestException(parsed["message"].Value<string>());

                return new YandexResult(text, source, target, parsed);
            }
        }

        async Task<TranslationBase> ITranslationService.TranslateAsync(string text, string target, string source)
            => await TranslateAsync(text, new YandexLanguage(target), new YandexLanguage(source));
    }
}
