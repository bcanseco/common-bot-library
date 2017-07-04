using System;
using System.Collections.Generic;
using System.Linq;
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

        #region LanguageCodes dictionary
        public IDictionary<string, string> LanguageCodes { get; }
            = new Dictionary<string, string>
            {
                {"af", "Afrikaans"},
                {"am", "Amharic"},
                {"ar", "Arabic"},
                {"az", "Azerbaijani"},
                {"ba", "Bashkir"},
                {"be", "Belarusian"},
                {"bg", "Bulgarian"},
                {"bn", "Bengali"},
                {"bs", "Bosnian"},
                {"ca", "Catalan"},
                {"ceb", "Cebuano"},
                {"cs", "Czech"},
                {"cy", "Welsh"},
                {"da", "Danish"},
                {"de", "German"},
                {"el", "Greek"},
                {"en", "English"},
                {"eo", "Esperanto"},
                {"es", "Spanish"},
                {"et", "Estonian"},
                {"eu", "Basque"},
                {"fa", "Persian"},
                {"fi", "Finnish"},
                {"fr", "French"},
                {"ga", "Irish"},
                {"gd", "Scottish Gaelic"},
                {"gl", "Galician"},
                {"gu", "Gujarati"},
                {"he", "Hebrew"},
                {"hi", "Hindi"},
                {"hr", "Croatian"},
                {"ht", "Haitian"},
                {"hu", "Hungarian"},
                {"hy", "Armenian"},
                {"id", "Indonesian"},
                {"is", "Icelandic"},
                {"it", "Italian"},
                {"ja", "Japanese"},
                {"jv", "Javanese"},
                {"ka", "Georgian"},
                {"kk", "Kazakh"},
                {"km", "Khmer"},
                {"kn", "Kannada"},
                {"ko", "Korean"},
                {"ky", "Kyrgyz"},
                {"la", "Latin"},
                {"lb", "Luxembourgish"},
                {"lo", "Lao"},
                {"lt", "Lithuanian"},
                {"lv", "Latvian"},
                {"mg", "Malagasy"},
                {"mhr", "Mari"},
                {"mi", "Maori"},
                {"mk", "Macedonian"},
                {"ml", "Malayalam"},
                {"mn", "Mongolian"},
                {"mr", "Marathi"},
                {"mrj", "Hill Mari"},
                {"ms", "Malay"},
                {"mt", "Maltese"},
                {"my", "Burmese"},
                {"ne", "Nepali"},
                {"nl", "Dutch"},
                {"no", "Norwegian"},
                {"pa", "Punjabi"},
                {"pap", "Papiamento"},
                {"pl", "Polish"},
                {"pt", "Portuguese"},
                {"ro", "Romanian"},
                {"ru", "Russian"},
                {"si", "Sinhalese"},
                {"sk", "Slovak"},
                {"sl", "Slovenian"},
                {"sq", "Albanian"},
                {"sr", "Serbian"},
                {"su", "Sundanese"},
                {"sv", "Swedish"},
                {"sw", "Swahili"},
                {"ta", "Tamil"},
                {"te", "Telugu"},
                {"tg", "Tajik"},
                {"th", "Thai"},
                {"tl", "Tagalog"},
                {"tr", "Turkish"},
                {"tt", "Tatar"},
                {"udm", "Udmurt"},
                {"uk", "Ukrainian"},
                {"ur", "Urdu"},
                {"uz", "Uzbek"},
                {"vi", "Vietnamese"},
                {"xh", "Xhosa"},
                {"yi", "Yiddish"},
                {"zh", "Chinese"}
            };
        #endregion

        /// <summary>
        ///   Uses Yandex Translate to translate text to another language.
        /// </summary>
        /// <param name="text">The text to translate.</param>
        /// <param name="target">The language to translate to.</param>
        /// <param name="source">The language to translate from. Autodetected if null.</param>
        /// <remarks>Language params can be in code ("en") or name ("English") form.</remarks>
        /// <exception cref="ArgumentNullException">
        ///   Thrown if <paramref name="text"/> or <paramref name="target"/> are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   Thrown if either <paramref name="target"/> or <paramref name="source"/> (if not null)
        ///   are unsupported languages (e.g. not in the <see cref="LanguageCodes"/> dictionary).
        /// </exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <seealso href="https://yandex.com/legal/translate_api/">TOS</seealso>
        public async Task<YandexResult> TranslateAsync(string text, string target, string source = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null.");
            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException(nameof(target), "Target language cannot be null.");

            var languageParam = GetLanguageParameter(source, target);

            using (var client = new RestClient("https://translate.yandex.net"))
            {
                var resource = $"api/v1.5/tr.json/translate?key={ApiKey}" +
                               $"&text={text}&lang={languageParam}&options=1";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsed = JObject.Parse(response.Content);

                if (!response.IsSuccess)
                    throw new HttpRequestException(parsed["message"].Value<string>());

                return new YandexResult(parsed);
            }
        }

        async Task<TranslationBase> ITranslationService.TranslateAsync(string text, string target, string source)
            => await TranslateAsync(text, target, source);

        #region Language Parameter generator
        /// <summary>
        ///   Returns a string for the 'lang' parameter of the Yandex Translate endpoint.
        /// </summary>
        /// <param name="source">The source language name or code (can be null).</param>
        /// <param name="target">The target language name or code.</param>
        /// <returns>[source-code]-[target-code], or just [target-code] if source is null.</returns>
        /// <example>
        ///   If source is "en" and target is "Spanish", the return value will be "en-es".
        ///   If source is "English" and target is "es", the return value will be "en-es".
        ///   If source is null and target is "es", the return value will be "es".
        /// </example>
        /// <exception cref="ArgumentException">
        ///   Thrown if either the target or source (if not null) params are not found in
        ///   either the key or value collections of the <see cref="LanguageCodes"/> dictionary.
        /// </exception>
        private string GetLanguageParameter(string source, string target)
        {
            // Make sure target refers to a language code and not a language name
            if (!LanguageCodes.TryGetValue(target, out string _))
            {
                target = LanguageCodes
                             .Where(c => c.Value == target)
                             .Select(l => l.Key)
                             .SingleOrDefault()
                         ?? throw new ArgumentException("Target language not supported.", target);
            }

            string langParam;

            if (source != null)
            {
                if (!LanguageCodes.TryGetValue(source, out string _))
                {
                    source = LanguageCodes
                                 .Where(c => c.Value == source)
                                 .Select(l => l.Key)
                                 .SingleOrDefault()
                             ?? throw new ArgumentException("Source language not supported.", source);
                }
                langParam = $"{source}-{target}";
            }
            else
            {
                langParam = target; // attempt language detection
            }

            return langParam;
        }
        #endregion
    }
}
