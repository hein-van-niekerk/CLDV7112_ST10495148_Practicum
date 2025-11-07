using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10495148_Practicum.Data;
using ST10495148_Practicum.Services;
using System;
using System.Collections.Generic;

namespace ST10495148_Practicum.Pages
{
    public class DashboardModel : PageModel
    {
        public int TotalEvents { get; set; }
        public int ProcessedEvents { get; set; }
        public int PendingMessages { get; set; }
        public List<Metric> RecentMetrics { get; set; } = new List<Metric>();
        public List<Event> RecentEvents { get; set; } = new List<Event>();

        public void OnGet()
        {
            // Example initialization. Replace with real data fetching logic.
            TotalEvents = 100;
            ProcessedEvents = 80;
            PendingMessages = 20;
            RecentMetrics = new List<Metric>
            {
                new Metric { MetricName = "CPU Usage", Value = 75, Unit = "%", Timestamp = DateTime.Now },
                new Metric { MetricName = "Memory Usage", Value = 2048, Unit = "MB", Timestamp = DateTime.Now }
            };
            RecentEvents = new List<Event>
            {
                new Event { EventType = "Login", Source = "Web", IsProcessed = true, Timestamp = DateTime.Now },
                new Event { EventType = "Logout", Source = "API", IsProcessed = false, Timestamp = DateTime.Now }
            };
        }
    }

    public class Metric
    {
        public string MetricName { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Event
    {
        public string EventType { get; set; }
        public string Source { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
