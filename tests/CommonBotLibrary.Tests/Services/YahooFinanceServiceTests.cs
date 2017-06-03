using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class YahooFinanceServiceTests
    {
        private YahooFinanceService Service { get; set; }

        [TestInitialize]
        public void InitService()
        {
            Service = new YahooFinanceService();
        }

        [TestMethod]
        public async Task Get_Symbols_Should_Work_With_Valid_Company()
        {
            var symbols = await Service.SearchSymbolsAsync("google");
            Assert.IsNotNull(symbols.First().Name);
        }

        [TestMethod]
        public async Task Get_Symbols_Should_Return_Empty_With_Invalid_Company()
        {
            var symbols = await Service.SearchSymbolsAsync("././");
            Assert.IsFalse(symbols.Any());
        }

        [TestMethod]
        public async Task Get_Quote_Should_Work_With_Valid_Symbol()
        {
            var quote = await Service.GetQuoteAsync("GOOG");
            Assert.IsNotNull(quote.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Get_Quote_Should_Fail_With_Invalid_Symbol()
        {
            var quote = await Service.GetQuoteAsync("././");
        }
    }
}
