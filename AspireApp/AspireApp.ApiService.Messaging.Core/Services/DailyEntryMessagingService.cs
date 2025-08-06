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
                await ProcessDailyEntryMessageAsync(message);
            }
        }

        public async Task ProcessDailyEntryMessageAsync(OutboxDailyEntry message)
        {
            try
            {
                var success = await _dailyEntryEventer.DailyEntryChangedAsync(message.Entry);
                if (success)
                {
                    await _repository.MessageSuccessfullySentAsync(message);
                }
                else
                {
                    await _repository.MessageFailedToSendAsync(message);
                }
            }
            catch
            {
                // log this exception
                await _repository.MessageFailedToSendAsync(message);
            }
        }
    }
}
