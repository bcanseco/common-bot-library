using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;

namespace CommonBotLibrary.Services
{
    public class GoogleVisionService : IVisionService
    {
        /// <summary>
        ///   Constructs an <see cref="IVisionService"/> implementation that uses Google Cloud Vision.
        /// </summary>
        /// <param name="platformKey">Defaults to platform key in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public GoogleVisionService(string platformKey = null)
        {
            PlatformKey = platformKey ?? Tokens.Google?.PlatformKey;

            if (string.IsNullOrWhiteSpace(PlatformKey))
            {
                var msg = $"Google token is required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string PlatformKey { get; }

        /// <summary>
        ///   Uses Google Cloud Vision to analyze an image.
        /// </summary>
        /// <param name="imageUrl">A direct link to an image.</param>
        /// <returns>Analysis results.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="Google.GoogleApiException">
        ///   Thrown if Google is down or rejects your token.
        /// </exception>
        /// <exception cref="ResultNotFoundException">
        ///   Thrown if the image cannot be found or processed.
        /// </exception>
        /// <seealso href="https://cloud.google.com/terms/service-terms">TOS</seealso>
        public async Task<AnnotateImageResponse> AnalyzeAsync(string imageUrl)
        {
            var visionService = new VisionService(
                new BaseClientService.Initializer { ApiKey = PlatformKey });

            var batch = new BatchAnnotateImagesRequest
            {
                Requests = new List<AnnotateImageRequest>
                {
                    new AnnotateImageRequest
                    {
                        Image = new Image {Source = new ImageSource {ImageUri = imageUrl}},
                        Features = new List<Feature>
                        {
                            new Feature {Type = "FACE_DETECTION"},
                            new Feature {Type = "LANDMARK_DETECTION"},
                            new Feature {Type = "LOGO_DETECTION"},
                            new Feature {Type = "LABEL_DETECTION"},
                            new Feature {Type = "TEXT_DETECTION"},
                            new Feature {Type = "DOCUMENT_TEXT_DETECTION"},
                            new Feature {Type = "SAFE_SEARCH_DETECTION"},
                            new Feature {Type = "IMAGE_PROPERTIES"},
                            new Feature {Type = "CROP_HINTS"},
                            new Feature {Type = "WEB_DETECTION"}
                        }
                    }
                }
            };

            var result = (await visionService.Images.Annotate(batch).ExecuteAsync()).Responses[0];

            if (result.Error != null)
                throw new ResultNotFoundException(result.Error.Message);

            return result;
        }

        async Task<ImageAnalysisBase> IVisionService.AnalyzeAsync(string imageUrl)
            => new GoogleVisionResult(await AnalyzeAsync(imageUrl));
    }
}
