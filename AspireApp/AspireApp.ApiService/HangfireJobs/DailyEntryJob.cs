using AspireApp.ApiService.Messaging;
using Hangfire;

namespace AspireApp.ApiService.HangfireJobs
{
    public class DailyEntryJob
    {
        private readonly IDailyEntryMessagingService _service;

        public DailyEntryJob(IDailyEntryMessagingService service)
        {
            _service = service;
        }

        public async Task ProcessDailyEntryMessagesAsync()
        {
            await _service.ProcessDailyEntryMessagesAsync();
        }

        public static void RegisterWithHangfire()
        {
            RecurringJob.AddOrUpdate(
                "daily_entries_messaging",
                (DailyEntryJob job) => job.ProcessDailyEntryMessagesAsync(),
                // Every 20 seconds
                "*/20 * * * * *"
            );
        }
    }
}
