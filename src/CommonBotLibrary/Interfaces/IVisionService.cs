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
        /// <param name="image">An image to analyze.</param>
        /// <returns>The analysis results.</returns>
        Task<ImageAnalysisBase> AnalyzeAsync(string image);
    }
}
