using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Repositories
{
    public interface IDailyEntryOutboxRepository
    {
        Task AddAsync(DailyEntryWithId entry);
    }
}
