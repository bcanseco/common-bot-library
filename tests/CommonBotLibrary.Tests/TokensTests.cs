using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CommonBotLibrary.Tests
{
    [TestClass]
    public class TokensTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task Should_Throw_Exception_If_Not_Found()
        {
            await Tokens.LoadAsync("~");
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public async Task Should_Throw_Exception_If_Invalid_JSON()
        {
            await Tokens.LoadAsync("../../../TestData/invalid.json");
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_JSON()
        {
            // contains "MyCustomToken" key-value pair
            const string validPath = "../../../TestData/valid.json";

            await Tokens.LoadAsync(validPath);
            Assert.IsNull(MyTokens.MyCustomToken);

            await Tokens.LoadAsync<MyTokens>(validPath);
            Assert.IsNotNull(MyTokens.MyCustomToken);
        }
    }

    public class MyTokens : Tokens
    {
        [JsonProperty]
        public static string MyCustomToken { get; set; }
    }
}
