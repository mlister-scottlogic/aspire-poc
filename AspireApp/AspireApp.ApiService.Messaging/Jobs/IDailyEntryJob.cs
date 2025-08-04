namespace AspireApp.ApiService.Messaging.Core.Jobs
{
    public interface IDailyEntryJob
    {
        Task ProcessDailyEntriesAsync();
    }
}
