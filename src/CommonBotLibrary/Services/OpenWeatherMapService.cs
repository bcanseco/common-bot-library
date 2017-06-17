using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using CommonBotLibrary.Interfaces.Models;
using CommonBotLibrary.Services.Models;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using Unit = CommonBotLibrary.Interfaces.Models.WeatherBase.Unit;

namespace CommonBotLibrary.Services
{
    public class OpenWeatherMapService : IWeatherService
    {
        /// <summary>
        ///   Constructs an <see cref="IWeatherService"/> implementation that searches Google.
        /// </summary>
        /// <param name="apiKey">Defaults to API key in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public OpenWeatherMapService(string apiKey = null)
        {
            ApiKey = apiKey ?? Tokens.OpenWeatherMap;

            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                var msg = $"OpenWeatherMap key is required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string ApiKey { get; }

        /// <summary>
        ///   Gets current weather data for a city from openweathermap.com
        /// </summary>
        /// <param name="city">The city to lookup weather for.</param>
        /// <param name="units">The units for temperature, etc.</param>
        /// <returns>Current weather conditions.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="InvalidCredentialsException">
        ///   Thrown if the OpenWeatherMap API key is invalid.
        /// </exception>
        /// <exception cref="ResultNotFoundException">
        ///   Thrown if the city name is not found.
        /// </exception>
        /// <seealso href="https://openweathermap.org/price">API Info</seealso>
        public async Task<OpenWeatherMapResult> GetCurrentWeatherAsync(string city, Unit units = default(Unit))
        {
            using (var client = new RestClient("http://api.openweathermap.org"))
            {
                var resource =
                    $"data/2.5/weather?q={city}&units={units}&appid={ApiKey}";

                var request = new RestRequest(resource, Method.GET);
                var response = await client.ExecuteAsync(request);

                var data = JObject.Parse(response.Content);

                return new OpenWeatherMapResult(data, units);
            }
        }

        async Task<WeatherBase> IWeatherService.GetCurrentWeatherAsync(string city)
            => await GetCurrentWeatherAsync(city);
    }
}
