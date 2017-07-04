using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json.Linq;

namespace CommonBotLibrary.Services.Models
{
    public class YandexResult : TranslationBase
    {
        public YandexResult(
            string originalText,
            YandexLanguage source,
            YandexLanguage target,
            JToken response)
        {
            InputText = originalText;
            OutputText = response["text"][0].Value<string>();

            IsSourceDetected = source == null;
            SourceLanguage = source ?? new YandexLanguage(response["detected"]["lang"].Value<string>());
            TargetLanguage = target;
            
        }

        public bool IsSourceDetected { get; }
        public YandexLanguage SourceLanguage { get; }
        public YandexLanguage TargetLanguage { get; }
    }
}
