using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for retrieving webpages.
    /// </summary>
    public interface IWebpageService
    {
        /// <summary>
        ///   Searches for relevant webpages for a given query.
        /// </summary>
        /// <param name="query">The search term to use.</param>
        /// <returns>A collection of relevant webpages.</returns>
        Task<IEnumerable<IWebpage>> SearchAsync(string query);
    }
}
