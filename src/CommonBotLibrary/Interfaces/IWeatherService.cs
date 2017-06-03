using System.Threading.Tasks;
using CommonBotLibrary.Interfaces.Models;

namespace CommonBotLibrary.Interfaces
{
    public interface IWeatherService
    {
        /// <summary>
        ///   Fetches the current weather conditions for a city.
        /// </summary>
        /// <param name="city">The city to get weather for.</param>
        /// <returns>Current weather data.</returns>
        /// <exception cref="Exceptions.ResultNotFoundException"></exception>
        Task<WeatherBase> GetCurrentWeatherAsync(string city);
    }
}
