using AspireApp.ApiService.Common;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Repositories
{
    public interface IDailyEntryRepository
    {
        Task<DailyEntryWithId> AddAsync(DailyEntry entry);
        Task<Optional<DailyEntryWithId>> GetByIdAsync(Guid id);
    }
}
