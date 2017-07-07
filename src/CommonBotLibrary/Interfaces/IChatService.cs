using System.Threading.Tasks;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for chatting with AIs.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        ///   Converses with an AI.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>The AI's response.</returns>
        Task<string> ConverseAsync(string message);
    }
}
