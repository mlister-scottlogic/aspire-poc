namespace AspireApp.ApiService.Messaging.Core.Jobs
{
    internal sealed class DailyEntryJob : IDailyEntryJob
    {
        public Task ProcessDailyEntriesAsync()
        {
            // Get all entries

            // for each {
            // Map to message
            // Publish
            // Delete from database if success
            // Save entries
            // }

            throw new NotImplementedException();
        }
    }
}
