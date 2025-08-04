using AspireApp.ApiService.Common;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Domain.Services
{
    public interface IDailyEntryService
    {
        Task<DailyEntryWithId> AddEntryAsync(DailyEntry entry);

        Task<Optional<DailyEntryWithId>> GetDailyEntry(Guid id);
    }
}
