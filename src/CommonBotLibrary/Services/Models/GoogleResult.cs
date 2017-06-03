using System.Diagnostics;
using CommonBotLibrary.Interfaces.Models;
using Google.Apis.Customsearch.v1.Data;

namespace CommonBotLibrary.Services.Models
{
    [DebuggerDisplay("{Url, nq}")]
    public class GoogleResult : IWebpage
    {
        public GoogleResult(Result apiResult)
        {
            Title = apiResult.Title;
            Url = apiResult.FormattedUrl;

            ApiResult = apiResult;
        }

        /// <summary>
        ///   The wrapped model returned by Google with more webpage properties.
        /// </summary>
        public Result ApiResult { get; }
        public string Url { get; set; }
        public string Title { get; set; }

        public static implicit operator Result(GoogleResult googleResult)
            => googleResult.ApiResult;
    }
}
