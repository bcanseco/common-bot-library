using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for working with a dictionary.
    /// </summary>
    public interface IDictionaryService
    {
        /// <summary>
        ///   Searches for relevant definitions for a given phrase.
        /// </summary>
        /// <param name="phrase">The phrase to look up.</param>
        /// <returns>A collection of relevant definitions.</returns>
        Task<IEnumerable<DefinitionBase>> GetDefinitionsAsync(string phrase);
    }
}
