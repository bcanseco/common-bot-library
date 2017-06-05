using System;
using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = CommonBotLibrary.Services.Models.OpenTriviaDbCategory;
using Difficulty = CommonBotLibrary.Services.Models.OpenTriviaDbDifficulty;
using Type = CommonBotLibrary.Services.Models.OpenTriviaDbType;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class OpenTriviaDbServiceTests
    {
        private OpenTriviaDbService Service { get; set; }

        [TestInitialize]
        public void InitService()
        {
            Service = new OpenTriviaDbService();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Should_Fail_With_Invalid_Amount()
        {
            var trivia = await Service.GetTriviaAsync(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Should_Fail_When_Not_Enough_Questions()
        {
            const Category category = Category.ScienceNature;
            const Difficulty difficulty = Difficulty.Hard;
            const Type type = Type.Multiple;
            var trivia = await Service.GetTriviaAsync(50, category, difficulty, type);
        }

        [TestMethod]
        public async Task Should_Work_With_Typical_Query()
        {
            var trivia = (await Service.GetTriviaAsync()).ToList();
            Assert.IsTrue(trivia.Any());
            Assert.IsNotNull(trivia.First().Question);
        }
    }
}
