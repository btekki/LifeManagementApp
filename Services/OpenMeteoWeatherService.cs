using LifeManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherModel = LifeManagementApp.Models.WeatherInfo;

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

        public async Task<WeatherModel?> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var latitudeValue = latitude.ToString(CultureInfo.InvariantCulture);
                var longitudeValue = longitude.ToString(CultureInfo.InvariantCulture);

                var url = $"{BaseUrl}?latitude={latitudeValue}"+$"&longitude={longitudeValue}"+$"&current=temperature_2m,wind_speed_10m"+ $"&wind_speed_unit=ms";

                using HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonContent = await response.Content.ReadAsStringAsync();

                OpenMeteoWeatherResponse? weatherResponse = JsonSerializer.Deserialize<OpenMeteoWeatherResponse>(jsonContent);

                if (weatherResponse?.Current is null)
                    return null;

                DateTime timestamp = DateTime.UtcNow;

                return new WeatherModel
                {
                    Temperature = weatherResponse.Current.Temperature,
                    WindSpeed = weatherResponse.Current.WindSpeed,
                    LastUpdated = timestamp,
                    Location = $"{latitudeValue},{longitudeValue}",
                    Latitude = latitude,
                    Longitude = longitude
                };
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Weather request error: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Weather JSON error: {ex.Message}");

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

        private sealed class OpenMeteoWeatherResponse
        {
            [JsonPropertyName("current")]
            public CurrentWeather? Current { get; set; }
        }
        private sealed class CurrentWeather
        {
            [JsonPropertyName("temperature_2m")]
            public double Temperature { get; set; }

            [JsonPropertyName("wind_speed_10m")]
            public double WindSpeed { get; set; }
        }
    }
}
