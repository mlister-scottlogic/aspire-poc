namespace AspireApp.ApiService.Messaging
{
    public class DailyEntryJob
    {
        private readonly IDailyEntryMessagingService _service;

        public DailyEntryJob(IDailyEntryMessagingService service)
        {
            _service = service;
        }

        public async Task ProcessDailyEntryMessages()
        {
            await _service.DoWorkAsync();
        }
    }
}
