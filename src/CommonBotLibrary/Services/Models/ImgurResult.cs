using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json;

namespace CommonBotLibrary.Services.Models
{
    public class ImgurResult : IWebpage
    {
        public string Id { get; set; }
        [JsonProperty("link")]
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonProperty("is_album")]
        public bool IsAlbum { get; set; }
    }
}
