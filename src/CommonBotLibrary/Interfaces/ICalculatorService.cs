using System.Threading.Tasks;

namespace CommonBotLibrary.Interfaces
{
    /// <summary>
    ///   Defines an API for numeric calculations.
    /// </summary>
    public interface ICalculatorService
    {
        /// <summary>
        ///   Evaluates a mathematical expression.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The evaluated result.</returns>
        /// <exception cref="System.ArgumentException">
        ///   Thrown if <paramref name="expression"/> cannot be evaluated.
        /// </exception>
        Task<string> EvaluateAsync(string expression);
    }
}
