using System;
using System.Diagnostics;

namespace CommonBotLibrary.Interfaces.Models
{
    [DebuggerDisplay("{Name, nq}: {TemperatureDisplay(), nq}")]
    public abstract class WeatherBase
    {
        public string Name { get; set; }
        public double Temperature { get; set; }
        public Unit UnitFormat { get; set; }
        
        public string TemperatureDisplay()
        {
            switch (UnitFormat)
            {
                case Unit.Imperial: return $"{Temperature}°F";
                case Unit.Metric:   return $"{Temperature}°C";
                case Unit.Kelvin:   return $"{Temperature}°K";
                default: throw new NotImplementedException($"Unknown unit format: {UnitFormat}.");
            }
        }

        public enum Unit
        {
            Imperial,
            Metric,
            Kelvin
        }
    }
}
