using AspireApp.ApiService.Common;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Repositories
{
    public interface IDailyEntryOutboxRepository
    {
        Task AddAsync(DailyEntryWithId entry);

        Task<IReadOnlyList<OutboxDailyEntry>> GetMessagesToProcessAsync();
        Task MessageSuccessfullySentAsync(OutboxDailyEntry dailyEntry);
        Task MessageFailedToSendAsync(OutboxDailyEntry dailyEntry);
        Task<Optional<OutboxDailyEntry>> GetMessageAsync(int id);

        Task<IReadOnlyCollection<OutboxDailyEntry>> GetFailedMessagesAsync();

        Task DeleteMessageAsync(int id);
    }

    public class OutboxDailyEntry
    {
        public required int Id { get; set; }
        public required int ProcessingAttempts { get; set; }
        public required DailyEntryWithId Entry { get; set; }
    }
}
