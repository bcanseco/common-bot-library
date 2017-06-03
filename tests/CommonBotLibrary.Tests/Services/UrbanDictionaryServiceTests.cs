using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class UrbanDictionaryServiceTests
    {
        private UrbanDictionaryService Service { get; set; }

        [TestInitialize]
        public void InitService()
        {
            Service = new UrbanDictionaryService();
        }

        [TestMethod]
        public async Task Should_Return_Empty_With_Invalid_Word()
        {
            var defs = await Service.GetDefinitionsAsync("aaa!@#$%^&*()zzz");

            Assert.IsFalse(defs.Any());
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Word()
        {
            var defs = (await Service.GetDefinitionsAsync("giraffe")).ToList();

            Assert.IsTrue(defs.Any());
            Assert.IsTrue(defs.First().DefId > 0);
        }
    }
}
