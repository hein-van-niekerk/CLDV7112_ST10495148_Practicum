namespace ST10495148_Practicum.Data
{
    public class PerformanceMetric
    {
        public int Id { get; set; }
        
        public string MetricName { get; set; } = string.Empty;
        
        public double Value { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        public string Unit { get; set; } = string.Empty;
        
        public string Source { get; set; } = string.Empty;
    }
}