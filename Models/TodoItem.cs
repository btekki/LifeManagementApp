using System;
using System.Collections.Generic;
using System.Text;

namespace LifeManagementApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public int Priority { get; set; } = 3;

        public DateTime? DueDate { get; set; }

        public bool IsDone { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Category { get; set; }

        public string? Notes { get; set; }
    }
}
