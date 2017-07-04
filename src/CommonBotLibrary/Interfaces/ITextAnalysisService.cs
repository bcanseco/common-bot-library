using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for analyzing text.
    /// </summary>
    public interface ITextAnalysisService
    {
        /// <summary>
        ///   Analyzes the specified text.
        /// </summary>
        /// <param name="text">The text to analyze.</param>
        /// <returns>Analysis results.</returns>
        Task<TextAnalysisBase> AnalyzeAsync(string text);
    }
}
