using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for analyzing images.
    /// </summary>
    public interface IVisionService
    {
        /// <summary>
        ///   Uses computer vision to analyze an image.
        /// </summary>
        /// <param name="imageUrl">A direct link to an image.</param>
        /// <returns>The analysis results.</returns>
        Task<ImageAnalysisBase> AnalyzeAsync(string imageUrl);
    }
}
