using System;
using System.Threading.Tasks;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalc;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class NCalcServiceTests
    {
        private NCalcService Service { get; set; }

        [TestInitialize]
        public void InitService()
        {
            Service = new NCalcService();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Should_Fail_With_Null_Expression()
        {
            var result = await Service.EvaluateAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Should_Fail_With_Blank_Expression()
        {
            var result = await Service.EvaluateAsync("");
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluationException))]
        public async Task Should_Fail_With_Invalid_Expression()
        {
            var result = await Service.EvaluateAsync("!@#$%");
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Expression()
        {
            var result = await Service.EvaluateAsync("2 + 2");
            Assert.AreEqual(4, int.Parse(result));
        }
    }
}
