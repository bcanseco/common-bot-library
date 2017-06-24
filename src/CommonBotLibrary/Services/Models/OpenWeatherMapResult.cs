using System.Diagnostics;
using CommonBotLibrary.Interfaces.Models;
using Newtonsoft.Json.Linq;

namespace CommonBotLibrary.Services.Models
{
    public class OpenWeatherMapResult : WeatherBase
    {
        public OpenWeatherMapResult(JToken json, Unit units)
        {
            Name = (string) json["name"];
            Temperature = (double) json["main"]["temp"];
            UnitFormat = units;
            CountryInitials = (string) json["sys"]["country"];
            Conditions = (string) json["weather"][0]["description"];
            Humidity = (int) json["main"]["humidity"];
            WindSpeed = (double) json["wind"]["speed"];
            Location = new Coordinates(json["coord"]["lat"], json["coord"]["lon"]);
        }
        
        public string CountryInitials { get; set; }
        public string Conditions { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public Coordinates Location { get; set; }

        [DebuggerDisplay("Lat: {Latitude, nq}, Long: {Longitude, nq}")]
        public class Coordinates
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public Coordinates(JToken lat, JToken lon)
            {
                Latitude = (double) lat;
                Longitude = (double)lon;
            }
        }
    }
}
