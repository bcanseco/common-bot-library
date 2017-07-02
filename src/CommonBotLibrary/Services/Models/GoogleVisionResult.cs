using System.Collections.Generic;
using System.Linq;
using CommonBotLibrary.Interfaces.Models;
using Google.Apis.Vision.v1.Data;

namespace CommonBotLibrary.Services.Models
{
    public class GoogleVisionResult : ImageAnalysisBase
    {
        public GoogleVisionResult(AnnotateImageResponse apiResult)
        {
            ApiResult = apiResult;
            Text = apiResult.FullTextAnnotation?.Text;
            DominantColors = apiResult.ImagePropertiesAnnotation?.DominantColors.Colors
                .Select(c => (c.Color.Red, c.Color.Green, c.Color.Blue))
                .ToList();
        }

        /// <summary>
        ///   The wrapped model returned by Google with more analysis properties.
        /// </summary>
        public AnnotateImageResponse ApiResult { get; }
        public IList<(float? R, float? G, float? B)> DominantColors { get; set; }

        public static implicit operator AnnotateImageResponse(GoogleVisionResult visionResult)
            => visionResult.ApiResult;
    }
}
