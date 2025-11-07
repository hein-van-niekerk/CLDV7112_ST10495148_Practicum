using System.Collections.Concurrent;

namespace ST10495148_Practicum.Services
{
    public class SimulatedEventHubService
    {
        private readonly ConcurrentQueue<string> _messages = new();

        public int MessageCount => _messages.Count;

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
    }
}
