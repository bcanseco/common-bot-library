using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonBotLibrary.Services.Models
{
    public class YandexLanguage
    {
        /// <summary>
        ///   A model wrapping a language supported by Yandex Translate.
        /// </summary>
        /// <param name="input">A language name or code.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">
        ///   Thrown if <paramref name="input"/> does not reference a supported language.
        /// </exception>
        public YandexLanguage(string input)
        {
            // Check if input is a language code
            if (LanguageCodes.TryGetValue(input, out string langName))
            {
                Code = input.ToLower();
                Name = langName;
            }
            // Check if input is a language name
            else
            {
                Code = LanguageCodes
                           .Where(c => c.Value.Equals(input, StringComparison.OrdinalIgnoreCase))
                           .Select(l => l.Key)
                           .SingleOrDefault()
                       ?? throw new ArgumentException("Language not supported.");
                Name = LanguageCodes[Code];
            }
        }

        public string Name { get; }
        public string Code { get; }

        public override string ToString() => Name;

        #region Supported languages
        public static IDictionary<string, string> LanguageCodes { get; }
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
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
    }
}
