using System.ComponentModel.DataAnnotations;

namespace ST10495148_Practicum.Data
{
    public class DataStreamEvent
    {
        public int Id { get; set; }

        [Required]
        public string EventType { get; set; } = string.Empty;

        [Required]
        public string Data { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string Source { get; set; } = string.Empty;

        public bool IsProcessed { get; set; } = false;

        public DateTime? ProcessedAt { get; set; }
    }
}