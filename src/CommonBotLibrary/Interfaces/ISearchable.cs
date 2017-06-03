using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for searching for data.
    /// </summary>
    public interface ISearchable<T>
        where T : class
    {
        /// <summary>
        ///   Searches for data matching the given query.
        /// </summary>
        /// <param name="query">The query to use for searching.</param>
        /// <returns>A collection of related search results.</returns>
        Task<IEnumerable<T>> SearchAsync(string query);
    }
}
