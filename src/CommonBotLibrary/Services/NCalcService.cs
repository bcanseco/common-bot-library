using System.Threading.Tasks;
using CommonBotLibrary.Interfaces;
using NCalc;

namespace CommonBotLibrary.Services
{
    public class NCalcService : ICalculatorService
    {
        /// <summary>
        ///   Returns the string result of a mathematical evaluation.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated expression.</returns>
        /// <exception cref="System.ArgumentException">
        ///   Thrown if <paramref name="expression"/> is empty or null.
        /// </exception>
        /// <exception cref="EvaluationException">
        ///   Thrown if NCalc encounters an error during evaluation.
        /// </exception>
        public Task<string> EvaluateAsync(string expression)
        {
            var result = new Expression(expression).Evaluate();
            return Task.FromResult(result.ToString());
        }
    }
}
