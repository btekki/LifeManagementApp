using System;
using System.Collections.Generic;
using System.Text;
using LifeManagementApp.Models;
using System.Text.Json.Serialization;

namespace LifeManagementApp.Services
{
    public class OpenMeteoWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

        public OpenMeteoWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherInfo?>GetCurrentWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{BaseUrl}?latitude{latitude}&longitude={longitude}&current=temperature_2m,wind_speed_10m&wind_speed_unit=ms";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                var weatherResponse = System.Text.Json.JsonSerializer.Deserialize<OpenMeteoWeatherResponse>(jsonContent);

                if (weatherResponse?.Current == null)
                    return null;

                return new WeatherInfo
                {
                    Temprature = weatherResponse.Current.Temperature,
                    WindSpeed = weatherResponse.Current.WindSpeed,
                    Latitude = latitude,
                    Longitude = longitude,
                    TimeStamp = DateTime.UtcNow
                };
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

        private class  OpenMeteoResponse
        {
            [JsonPropertyName("current")]
            public CurrentWeather? Current { get; set; }
        }
        private class  CurrentWeather
        {
            [JsonPropertyName("temperature_2m")]
            public double Temperature { get; set; }

            [JsonPropertyName("wind_speed_10m")]
            public double WindSpeed { get; set; }
        }
    }
}
