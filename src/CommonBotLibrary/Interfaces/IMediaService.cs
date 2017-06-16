using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for working with digital media.
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        ///   Searches for all media matching a given query.
        /// </summary>
        /// <param name="title">The media title to search with.</param>
        /// <returns>A collection of relevant media.</returns>
        Task<IEnumerable<MediaBase>> SearchAsync(string title);

        /// <summary>
        ///   Gets full information about a particular piece of media.
        /// </summary>
        /// <param name="title">The media title to match against.</param>
        /// <returns>The most relevant media for the given title.</returns>
        /// <exception cref="Exceptions.ResultNotFoundException"></exception>
        Task<MediaBase> DirectAsync(string title);
    }
}
