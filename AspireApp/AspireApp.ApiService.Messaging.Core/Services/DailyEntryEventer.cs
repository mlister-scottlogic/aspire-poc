using System.Text;
using System.Text.Json;
using AspireApp.ApiService.Domain.Models;
using RabbitMQ.Client;

namespace AspireApp.ApiService.Messaging.Core.Services
{
    internal sealed class DailyEntryEventer : IDailyEntryEventer
    {
        private const string DailyEntryQueue = "daily_entries";

        private readonly IConnection _rabbitConnection;

        public DailyEntryEventer(IConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public async Task<bool> DailyEntryChangedAsync(DailyEntryWithId entry)
        {
            using var channel = await _rabbitConnection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: DailyEntryQueue,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ConvertToContract(entry)));

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: DailyEntryQueue,
                body: body
            );

            return true;
        }

        private static Contracts.DailyEntryWithId ConvertToContract(DailyEntryWithId entry)
        {
            return new Contracts.DailyEntryWithId()
            {
                Id = entry.Id,
                Title = entry.Title,
                Distance = entry.Distance,
                Date = entry.Date,
                Description = entry.Description,
                DistanceUnit = Enum.Parse<Contracts.DistanceUnit>(entry.DistanceUnit.ToString()),
            };
        }
    }
}
