using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class YahooFinanceService : IStockService, ISearchable<SymbolBase>
    {
        /// <summary>
        ///   Searches the Yahoo SymbolSuggest API for symbols matching a company name.
        /// </summary>
        /// <param name="companyName">The company name to search with.</param>
        /// <returns>A collection of relevant symbols.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <seealso href="https://policies.yahoo.com/us/en/yahoo/terms/">TOS</seealso>
        public async Task<IEnumerable<YahooResult>> SearchSymbolsAsync(string companyName)
        {
            using (var client = new RestClient("http://d.yimg.com"))
            {
                var resource = $"aq/autoc?query={companyName.NoPunctuation()}&region=US&lang=en-US";
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var parsed = JObject.Parse(response.Content);

                if (!parsed["ResultSet"]["Result"].HasValues)
                    return Enumerable.Empty<YahooResult>();

                return parsed["ResultSet"]["Result"]
                    .Select(s => s.ToObject<YahooResult>());
            }
        }

        /// <summary>
        ///   Gets quote data for a given symbol from the Yahoo Finance API.
        /// </summary>
        /// <param name="symbol">The symbol to query for.</param>
        /// <returns>Stock quote data.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="ResultNotFoundException"></exception>
        /// <seealso href="https://policies.yahoo.com/us/en/yahoo/terms/">TOS</seealso>
        public async Task<YahooQuote> GetQuoteAsync(string symbol)
        {
            using (var client = new RestClient("https://query.yahooapis.com"))
            {
                const string table = "store://datatables.org/alltableswithkeys";
                var query = $"select * from yahoo.finance.quotes where symbol = \"{symbol}\" limit 1";
                var uri = $"v1/public/yql?q={query}&format=json&env={table}";

                var resource = Uri.EscapeUriString(uri);
                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);
                
                var quote = JObject.Parse(response.Content)["query"]["results"]["quote"];

                if (quote["Name"].Value<string>() == null)
                    throw new ResultNotFoundException($"No quote was found for \"{symbol}\".");

                return quote.ToObject<YahooQuote>();
            }
        }

        async Task<IEnumerable<SymbolBase>> IStockService.SearchSymbolsAsync(string companyName)
            => await SearchSymbolsAsync(companyName);

        async Task<IEnumerable<SymbolBase>> ISearchable<SymbolBase>.SearchAsync(string companyName)
            => await SearchSymbolsAsync(companyName);

        async Task<SymbolBase> IStockService.GetQuoteAsync(string symbol)
            => await GetQuoteAsync(symbol);
    }
}
