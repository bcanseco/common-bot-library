using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for working with anime.
    /// </summary>
    public interface IAnimeService
    {
        /// <summary>
        ///   Searches for all anime matching a given query.
        /// </summary>
        /// <param name="title">The anime title to search with.</param>
        /// <returns>A collection of relevant anime.</returns>
        Task<IEnumerable<AnimeBase>> SearchAsync(string title);
    }
}
