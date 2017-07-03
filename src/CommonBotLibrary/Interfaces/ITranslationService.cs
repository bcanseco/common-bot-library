using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for translating text.
    /// </summary>
    public interface ITranslationService
    {
        /// <summary>
        ///   Translates text to another language.
        /// </summary>
        /// <param name="text">The text to translate.</param>
        /// <param name="target">The language to translate to.</param>
        /// <param name="source">The language to translate from.</param>
        /// <returns>A model holding the translated text.</returns>
        Task<TranslationBase> TranslateAsync(string text, string target, string source);
    }
}
