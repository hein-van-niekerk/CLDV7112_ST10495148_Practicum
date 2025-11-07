using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10495148_Practicum.Data;
using ST10495148_Practicum.Services;

namespace ST10495148_Practicum.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SimulatedEventHubService _eventHubService;

        public IndexModel(ApplicationDbContext context, SimulatedEventHubService eventHubService)
        {
            _context = context;
            _eventHubService = eventHubService;
        }

        public int TotalEvents { get; set; }
        public int ProcessedEvents { get; set; }
        public int PendingMessages { get; set; }
        public List<PerformanceMetric> RecentMetrics { get; set; } = new();
        public List<DataStreamEvent> RecentEvents { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                // Get real data from database
                TotalEvents = await _context.DataStreamEvents.CountAsync();
                ProcessedEvents = await _context.DataStreamEvents.CountAsync(e => e.IsProcessed);
                PendingMessages = _eventHubService.MessageCount;
                
                RecentMetrics = await _context.PerformanceMetrics
                    .OrderByDescending(pm => pm.Timestamp)
                    .Take(10)
                    .ToListAsync();

                RecentEvents = await _context.DataStreamEvents
                    .OrderByDescending(e => e.Timestamp)
                    .Take(10)
                    .ToListAsync();
            }
            catch (Exception)
            {
                // Fallback to sample data if database is not available
                TotalEvents = 0;
                ProcessedEvents = 0;
                PendingMessages = 0;
                RecentMetrics = new List<PerformanceMetric>();
                RecentEvents = new List<DataStreamEvent>();
            }
        }
    }
}
