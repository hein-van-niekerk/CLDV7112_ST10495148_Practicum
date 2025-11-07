using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

namespace YourApp.Services
{
    public class EventHubService
    {
        private readonly EventHubProducerClient _producer;

        public EventHubService(IConfiguration config)
        {
            var connectionString = config["EventHub:ConnectionString"];
            var eventHubName = config["EventHub:Name"];
            _producer = new EventHubProducerClient(connectionString, eventHubName);
        }

        public async Task SendMessageAsync(string message)
        {
            using EventDataBatch batch = await _producer.CreateBatchAsync();
            batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message)));
            await _producer.SendAsync(batch);
        }
    }
}
