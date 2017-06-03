using System.Collections.Generic;
using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    public interface IStockService
    {
        /// <summary>
        ///   Searches for all symbols matching a given company name.
        /// </summary>
        /// <param name="companyName">The company name to search with.</param>
        /// <returns>A collection of relevant symbols.</returns>
        Task<IEnumerable<SymbolBase>> SearchSymbolsAsync(string companyName);

        /// <summary>
        ///   Gets quote data for a given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to use.</param>
        /// <returns>Stock quote data.</returns>
        /// <exception cref="Exceptions.ResultNotFoundException"></exception>
        Task<SymbolBase> GetQuoteAsync(string symbol);
    }
}
