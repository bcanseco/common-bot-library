using System.Linq;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services
{
    [TestClass]
    public class SteamServiceTests
    {
        private SteamService Service { get; set; }

        [TestInitialize]
        public void InitService()
        {
            Service = new SteamService();
        }

        [TestMethod]
        public async Task Should_Return_Empty_With_Unknown_Title()
        {
            var games = await Service.SearchAsync("1@3$mNbVuNyBtVrC");

            Assert.IsFalse(games.Any());
        }

        [TestMethod]
        public async Task Should_Work_With_Valid_Title()
        {
            var games = (await Service.SearchAsync("batman")).ToList();

            Assert.IsTrue(games.Any());
            Assert.IsNotNull(games.First().Title);
        }

        [TestMethod]
        public async Task Should_Get_SteamSpy_Data_With_Valid_Id()
        {
            var spyData = await Service.GetSteamSpyDataAsync(730);
            Assert.AreEqual(spyData.Name, "Counter-Strike: Global Offensive");
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Should_Fail_SteamSpy_Data_With_Invalid_Id()
        {
            var spyData = await Service.GetSteamSpyDataAsync(1);
        }

        [TestMethod]
        public async Task Should_Get_Players_With_Valid_Id()
        {
            var players = await Service.GetCurrentPlayersAsync(730);
            Assert.IsTrue(players > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ResultNotFoundException))]
        public async Task Should_Fail_Getting_Players_With_Invalid_Id()
        {
            var players = await Service.GetCurrentPlayersAsync(1);
        }
    }
}
