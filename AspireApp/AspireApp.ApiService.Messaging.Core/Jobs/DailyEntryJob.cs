namespace AspireApp.ApiService.Messaging.Core.Jobs
{
    public sealed class DailyEntryJob : IDailyEntryJob
    {
        public async Task ProcessDailyEntriesAsync()
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
