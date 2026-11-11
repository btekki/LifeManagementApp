using System;
using System.Collections.Generic;
using System.Text;

namespace LifeManagementApp.Models
{
    public class WeatherInfo
    {
        public int Id { get; set; }

        public required double Temperature { get; set; }

        public required double WindSpeed { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public string? Location { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
