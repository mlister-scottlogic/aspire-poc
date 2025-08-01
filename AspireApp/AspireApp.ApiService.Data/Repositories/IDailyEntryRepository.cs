using AspireApp.ApiService.Common;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Repositories
{
    public interface IDailyEntryRepository
    {
        Task<DailyEntry> AddAsync(DailyEntry entry);
        Task<Optional<DailyEntry>> GetByIdAsync(Guid id);
    }
}
