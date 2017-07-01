using System.Threading.Tasks;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for working with version control software.
    /// </summary>
    public interface IVersionControlService
    {
        /// <summary>
        ///   Creates a repository issue.
        /// </summary>
        /// <param name="title">The title to use.</param>
        /// <param name="body">The body to use.</param>
        /// <param name="labels">A collection of labels to apply.</param>
        /// <returns>True if the issue was successfully created, false otherwise.</returns>
        Task<bool> CreateIssueAsync(string title, string body, string[] labels);
    }
}
