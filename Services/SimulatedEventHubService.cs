using System.Collections.Concurrent;

namespace YourApp.Services
{
    public class SimulatedEventHubService
    {
        private readonly ConcurrentQueue<string> _messages = new();

        public void SendMessage(string message)
        {
            _messages.Enqueue(message);
        }

        public IEnumerable<string> ReceiveMessages(int maxCount = 10)
        {
            var messages = new List<string>();
            while (messages.Count < maxCount && _messages.TryDequeue(out var msg))
            {
                messages.Add(msg);
            }
            return messages;
        }

        public int MessageCount => _messages.Count;
    }
}
