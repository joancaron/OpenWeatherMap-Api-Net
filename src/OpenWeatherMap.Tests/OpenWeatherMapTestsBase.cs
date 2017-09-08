using System;

namespace OpenWeatherMap.Tests
{
    public class OpenWeatherMapTestsBase
    {
        public const string CityName = "London";
        public const string Metric = "metric";
        public const string Celsius = "celsius";
        public const double Latitude = 51.51;
        public const double Longitude = -0.13;
        public const int CityId = 2643743;

        public readonly OpenWeatherMapClient OpenWeatherMapTestClient = new OpenWeatherMapClient(Environment.GetEnvironmentVariable("OPENWEATHERMAP_APIKEY"));
    }
}
