using AspireApp.ApiService.Data.Repositories;

namespace AspireApp.ApiService.Messaging.Core.Services
{
    internal sealed class DailyEntryMessagingService : IDailyEntryMessagingService
    {
        private readonly IDailyEntryOutboxRepository _repository;
        private readonly IDailyEntryEventer _dailyEntryEventer;

        public DailyEntryMessagingService(
            IDailyEntryOutboxRepository repository,
            IDailyEntryEventer dailyEntryEventer
        )
        {
            _repository = repository;
            _dailyEntryEventer = dailyEntryEventer;
        }

        public async Task ProcessDailyEntryMessagesAsync()
        {
            var messagesToProcess = await _repository.GetMessagesToProcessAsync();

            foreach (var message in messagesToProcess)
            {
                try
                {
                    SendMessage(message);
                    await _repository.MessageSuccessfullySentAsync(message);
                }
                catch
                {
                    // log this exception
                    await _repository.MessageFailedToSendAsync(message);
                }
            }
        }

        private void SendMessage(OutboxDailyEntry entry)
        {
            if (entry.Id % 3 == 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
