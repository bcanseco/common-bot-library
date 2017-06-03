using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json;

namespace CommonBotLibrary.Services.Models
{
    public class UrbanDictionaryResult : DefinitionBase
    {
        public int DefId { get; set; }
        public string Permalink { get; set; }
        public string Example { get; set; }
        public string Author { get; set; }
        [JsonProperty("thumbs_up")]
        public int ThumbsUp { get; set; }
        [JsonProperty("thumbs_down")]
        public int ThumbsDown { get; set; }
    }
}
