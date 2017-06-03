using System;
using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json;

namespace CommonBotLibrary.Services.Models
{
    public class YahooResult : SymbolBase
    {
        [JsonProperty("exch")]
        public string StockExchange { get; set; }
        [JsonProperty("exchDisp")]
        public string ExchangeDisplay { get; set; }
        public string Type { get; set; }
        [JsonProperty("typeDisp")]
        public string TypeDisplay { get; set; }
    }

    public class YahooQuote : YahooResult
    {
        // Avoid conflict with base class JsonProperty
        public new string StockExchange
        {
            get => base.StockExchange;
            set => base.StockExchange = value;
        }
        public string Currency { get; set; }
        public string Open { get; set; }
        public string PreviousClose { get; set; }
        public string Ask { get; set; }
        public string Bid { get; set; }
        public string DaysLow { get; set; }
        public string DaysHigh { get; set; }
        public string YearLow { get; set; }
        public string YearHigh { get; set; }
        public string Change { get; set; }
        public string PercentChange { get; set; }
        public string AverageDailyVolume { get; set; }
        public string MarketCapitalization { get; set; }
        public string EarningsShare { get; set; }
        public string Volume { get; set; }
        public string Notes { get; set; }
        public DateTime LastTradeDate { get; set; }
    }
}
