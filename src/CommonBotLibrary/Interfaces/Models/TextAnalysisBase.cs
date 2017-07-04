using Newtonsoft.Json;

namespace CommonBotLibrary.Interfaces.Models
{
    public abstract class TextAnalysisBase
    {
        [JsonProperty("word_count")]
        public long WordCount { get; set; }
        [JsonProperty("processed_language")]
        public string ProcessedLanguage { get; set; }
    }
}
