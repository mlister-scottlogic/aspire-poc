using AspireApp.ApiService.Data.Repositories;

namespace AspireApp.ApiService.Messaging
{
    public interface IDailyEntryMessagingService
    {
        Task ProcessDailyEntryMessagesAsync();

        Task ProcessDailyEntryMessageAsync(OutboxDailyEntry entry);
    }
}
