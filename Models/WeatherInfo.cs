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

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public string? Location { get; set; }
    }
}
