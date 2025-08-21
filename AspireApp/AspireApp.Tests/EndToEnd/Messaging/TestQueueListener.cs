using System.Text.Json;
using AspireApp.ApiService.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AspireApp.Tests.EndToEnd.Messaging
{
    public class TestQueueListener : IDisposable
    {
        private readonly IConnection _connection;

        private readonly IDictionary<Guid, IList<DailyEntryWithId>> _entryMessages;

        public TestQueueListener(IConnection connection)
        {
            _connection = connection;
            _entryMessages = new Dictionary<Guid, IList<DailyEntryWithId>>();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task StartListeningToMessagesAsync()
        {
            var channel = await _connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();

                var dailyEntry = JsonSerializer.Deserialize<DailyEntryWithId>(body);

                if (_entryMessages.TryGetValue(dailyEntry.Id.Value, out var list))
                {
                    list.Add(dailyEntry);
                }
                else
                {
                    _entryMessages.Add(dailyEntry.Id.Value, [dailyEntry]);
                }

                OnMessageReceived(dailyEntry);

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await channel.QueueDeclareAsync(
                "daily_entries",
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            string consumerTag = await channel.BasicConsumeAsync("daily_entries", false, consumer);
        }

        public async Task<IList<DailyEntryWithId>?> GetMessagesForIdAsync(Guid id)
        {
            if (_entryMessages.TryGetValue(id, out var list))
            {
                return list;
            }

            var cancelationToken = new CancellationTokenSource();

            MessageReceived += (o, e) =>
            {
                if (e.Id == id)
                {
                    cancelationToken.Cancel();
                }
            };

            // Wait for up to 30 seconds or until the cancellation token is cancelled by
            // the MessageReceived event handler
            await Task.Delay(TimeSpan.FromSeconds(30), cancelationToken.Token)
                .ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

            if (_entryMessages.TryGetValue(id, out var newList))
            {
                return newList;
            }

            return null;
        }

        public event EventHandler<DailyEntryWithId>? MessageReceived;

        protected virtual void OnMessageReceived(DailyEntryWithId e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}
