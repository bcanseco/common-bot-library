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
        /// <param name="conversationId">The conversation context to send the message in.</param>
        /// <returns>The AI's response, and the current conversation ID.</returns>
        Task<(string Reply, string ConversationId)> ConverseAsync(string message, string conversationId);
    }
}
