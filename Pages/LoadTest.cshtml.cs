using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ST10495148_Practicum.Services;
using System.Text.Json;

namespace ST10495148_Practicum.Pages
{
    public class LoadTestModel : PageModel
    {
        private readonly SimulatedEventHubService _eventHubService;

        public LoadTestModel(SimulatedEventHubService eventHubService)
        {
            _eventHubService = eventHubService;
        }

        [BindProperty]
        public int MessageCount { get; set; } = 100;

        public string Status { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var random = new Random();
                var eventTypes = new[] { "order", "inventory", "user_action", "system_event" };

                for (int i = 0; i < MessageCount; i++)
                {
                    var eventData = new
                    {
                        eventType = eventTypes[random.Next(eventTypes.Length)],
                        id = Guid.NewGuid(),
                        timestamp = DateTime.UtcNow,
                        data = $"Test data {i + 1}"
                    };

                    _eventHubService.SendMessage(JsonSerializer.Serialize(eventData));
                }

                Status = $"✅ Successfully generated {MessageCount} test messages!";
            }
            catch (Exception ex)
            {
                Status = $"❌ Error: {ex.Message}";
            }

            return Page();
        }
    }
}