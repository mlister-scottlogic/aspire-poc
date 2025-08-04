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
            Console.WriteLine("Starting job");

            await Task.Delay(10);
            // Get all entries

            // for each {
            // Map to message
            // Publish
            // Delete from database if success
            // Save entries
            // }

            Console.WriteLine("Finishing job");

            //throw new NotImplementedException();
        }
    }
}
