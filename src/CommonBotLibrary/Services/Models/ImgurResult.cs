using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json;

namespace CommonBotLibrary.Services.Models
{
    public class ImgurResult : IWebpage
    {
        [JsonProperty("link")]
        public string Url { get; set; }
        public string Title { get; set; }

        public string Id { get; set; }
        public string Description { get; set; }
        [JsonProperty("is_album")]
        public bool IsAlbum { get; set; }
    }
}
