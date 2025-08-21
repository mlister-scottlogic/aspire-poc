using System.Text.Json;
using AspireApp.ApiService.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AspireApp.Tests.EndToEnd.Messaging
{
    public class TestQueueListener : IDisposable
    {
        private readonly IConnection _connection;

        public TestQueueListener(IConnection connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task ListenForMessagesAsync()
        {
            var channel = await _connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();

                var dailyEntry = JsonSerializer.Deserialize<DailyEntryWithId>(body);

                Console.WriteLine("DailyEntry Event:");
                Console.WriteLine(dailyEntry.Id);
                Console.WriteLine(dailyEntry.Title);

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await channel.QueueDeclareAsync(
                "daily_entries",
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            string consumerTag = await channel.BasicConsumeAsync("daily_entries", false, consumer);

            //await Task.Delay(30_000);
        }
    }
}
