using System.Threading.Tasks;
using CommonBotLibrary.Services;
using CommonBotLibrary.Services.Models;

namespace CommonBotLibrary.Extensions
{
    public static class SteamServiceExtensions
    {
        /// <summary>
        ///   Overload for <see cref="SteamService.GetSteamSpyDataAsync(uint)"/>
        ///   that allows searches with the given game's Id property.
        /// </summary>
        public static async Task<SteamSpyData> GetSteamSpyDataAsync(this SteamService service, SteamResult game)
            => await service.GetSteamSpyDataAsync(game.Id);

        /// <summary>
        ///   Overload for <see cref="SteamService.GetCurrentPlayersAsync(uint)"/>
        ///   that allows retrieval with the given game's Id property.
        /// </summary>
        public static async Task<uint> GetCurrentPlayersAsync(this SteamService service, SteamResult game)
            => await service.GetCurrentPlayersAsync(game.Id);

        /// <summary>
        ///   Overload for <see cref="SteamService.GetCurrentPlayersAsync(uint)"/>
        ///   that allows retrieval with the given SteamSpy data's AppId property.
        /// </summary>
        public static async Task<uint> GetCurrentPlayersAsync(this SteamService service, SteamSpyData data)
            => await service.GetCurrentPlayersAsync(data.AppId);
    }
}
