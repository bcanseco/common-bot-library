using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    public interface ITriviaService
    {
        /// <summary>
        ///   Gets trivia questions.
        /// </summary>
        /// <param name="amount">The amount of results to retrieve.</param>
        /// <returns>A collection of relevant trivia.</returns>
        Task<IEnumerable<TriviaBase>> GetTriviaAsync(int amount);
    }
}
