using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json.Linq;

namespace CommonBotLibrary.Services.Models
{
    public class YandexResult : TranslationBase
    {
        public YandexResult(JToken response)
        {
            OutputText = response["text"][0].Value<string>();

            DetectedLanguage = response["detected"]["lang"].Value<string>();
            LanguageFlow = response["lang"].Value<string>();
        }

        public string DetectedLanguage { get; }
        public string LanguageFlow { get; }
    }
}
