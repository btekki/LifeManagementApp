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

        public async Task<WeatherInfo?> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{BaseUrl}?latitude={latitude}&longitude={longitude}&current=temperature_2m,wind_speed_10m&wind_speed_unit=ms";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                var weatherResponse = System.Text.Json.JsonSerializer.Deserialize<OpenMeteoWeatherResponse>(jsonContent);

                if (weatherResponse?.Current == null)
                    return null;

                return new WeatherInfo
                {
                    Temperature = weatherResponse.Current.Temperature,
                    WindSpeed = weatherResponse.Current.WindSpeed,
                    LastUpdated = DateTime.UtcNow,
                    Location = $"{latitude},{longitude}"
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

        private class OpenMeteoWeatherResponse
        {
            [JsonPropertyName("current")]
            public CurrentWeather? Current { get; set; }
        }
        private class CurrentWeather
        {
            [JsonPropertyName("temperature_2m")]
            public double Temperature { get; set; }

            [JsonPropertyName("wind_speed_10m")]
            public double WindSpeed { get; set; }
        }
    }
}
public class WeatherInfo
{
  public int Id { get; set; }
  public required double Temperature { get; set; }
  public required double WindSpeed { get; set; }
  public DateTime LastUpdated { get; set; }
  public string? Location { get; set; }

  // Add these if other code expects them
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public DateTime Timestamp { get; set; }
}
