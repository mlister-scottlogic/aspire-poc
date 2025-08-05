using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Repositories
{
    public interface IDailyEntryOutboxRepository
    {
        Task AddAsync(DailyEntryWithId entry);

        Task<IReadOnlyList<OutboxDailyEntry>> GetMessagesToProcessAsync();
        Task MessageSuccessfullySentAsync(OutboxDailyEntry dailyEntry);
        Task MessageFailedToSendAsync(OutboxDailyEntry dailyEntry);
    }

    public class OutboxDailyEntry
    {
        public int Id { get; set; }
        public required DailyEntryWithId Entry { get; set; }
    }
}
