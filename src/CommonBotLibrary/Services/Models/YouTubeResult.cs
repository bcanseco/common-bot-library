using System.Diagnostics;
using CommonBotLibrary.Interfaces.Models;
using Google.Apis.YouTube.v3.Data;

namespace CommonBotLibrary.Services.Models
{
    [DebuggerDisplay("{Title, nq}")]
    public class YouTubeResult : IWebpage
    {
        public YouTubeResult(SearchResult result)
        {
            Url = $"https://youtube.com/watch?v={result.Id.VideoId}";
            Title = result.Snippet.Title;

            ApiResult = result;
        }

        /// <summary>
        ///   The wrapped model returned by YouTube with more video properties.
        /// </summary>
        public SearchResult ApiResult { get; }
        public string Url { get; set; }
        public string Title { get; set; }

        public static implicit operator SearchResult(YouTubeResult result)
            => result.ApiResult;
    }
}
