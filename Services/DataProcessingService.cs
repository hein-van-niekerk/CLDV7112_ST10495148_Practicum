using Microsoft.EntityFrameworkCore;
using ST10495148_Practicum.Data;
using System.Text.Json;

namespace ST10495148_Practicum.Services
{
    public class DataProcessingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SimulatedEventHubService _eventHubService;
        private readonly ILogger<DataProcessingService> _logger;

        public DataProcessingService(
            IServiceProvider serviceProvider,
            SimulatedEventHubService eventHubService,
            ILogger<DataProcessingService> logger)
        {
            _serviceProvider = serviceProvider;
            _eventHubService = eventHubService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessDataStreams();
                    await Task.Delay(1000, stoppingToken); // Process every second
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing data streams");
                    await Task.Delay(5000, stoppingToken); // Wait longer on error
                }
            }
        }

        private async Task ProcessDataStreams()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Get unprocessed events from the queue
            var messages = _eventHubService.ReceiveMessages(50);

            if (!messages.Any()) return;

            foreach (var message in messages)
            {
                try
                {
                    // Parse and process the message
                    var eventData = JsonSerializer.Deserialize<Dictionary<string, object>>(message);

                    var dataEvent = new DataStreamEvent
                    {
                        EventType = eventData?.ContainsKey("eventType") == true ? eventData["eventType"]?.ToString() ?? "Unknown" : "Unknown",
                        Data = message,
                        Source = "SimulatedEventHub",
                        Timestamp = DateTime.UtcNow
                    };

                    context.DataStreamEvents.Add(dataEvent);

                    // Simulate processing logic
                    await SimulateProcessing(dataEvent, context);

                    dataEvent.IsProcessed = true;
                    dataEvent.ProcessedAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message: {Message}", message);
                }
            }

            await context.SaveChangesAsync();
        }

        private async Task SimulateProcessing(DataStreamEvent dataEvent, ApplicationDbContext context)
        {
            // Simulate different processing based on event type
            switch (dataEvent.EventType.ToLower())
            {
                case "order":
                    await ProcessOrderEvent(dataEvent, context);
                    break;
                case "inventory":
                    await ProcessInventoryEvent(dataEvent, context);
                    break;
                default:
                    await ProcessGenericEvent(dataEvent, context);
                    break;
            }
        }

        private async Task ProcessOrderEvent(DataStreamEvent dataEvent, ApplicationDbContext context)
        {
            // Record performance metric
            var metric = new PerformanceMetric
            {
                MetricName = "OrderProcessingTime",
                Value = Random.Shared.Next(50, 200), // Simulate processing time in ms
                Unit = "milliseconds",
                Source = "DataProcessingService"
            };

            context.PerformanceMetrics.Add(metric);
            await Task.Delay(10); // Simulate processing time
        }

        private async Task ProcessInventoryEvent(DataStreamEvent dataEvent, ApplicationDbContext context)
        {
            var metric = new PerformanceMetric
            {
                MetricName = "InventoryUpdateTime",
                Value = Random.Shared.Next(20, 100),
                Unit = "milliseconds",
                Source = "DataProcessingService"
            };

            context.PerformanceMetrics.Add(metric);
            await Task.Delay(5);
        }

        private async Task ProcessGenericEvent(DataStreamEvent dataEvent, ApplicationDbContext context)
        {
            var metric = new PerformanceMetric
            {
                MetricName = "GenericProcessingTime",
                Value = Random.Shared.Next(10, 50),
                Unit = "milliseconds",
                Source = "DataProcessingService"
            };

            context.PerformanceMetrics.Add(metric);
            await Task.Delay(2);
        }
    }
}
