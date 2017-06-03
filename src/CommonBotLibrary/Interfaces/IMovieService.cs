using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for working with movies.
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        ///   Searches for all movies matching a given query.
        /// </summary>
        /// <param name="title">The movie title to search with.</param>
        /// <returns>A collection of relevant movies.</returns>
        Task<IEnumerable<MovieBase>> SearchAsync(string title);

        /// <summary>
        ///   Gets full information about a particular movie.
        /// </summary>
        /// <param name="title">The movie title to match against.</param>
        /// <returns>The most relevant movie for the given title.</returns>
        /// <exception cref="Exceptions.ResultNotFoundException"></exception>
        Task<MovieBase> DirectAsync(string title);
    }
}
