using AspireApp.ApiService.Common;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Domain.Services
{
    public interface IDailyEntryService
    {
        Task<DailyEntry> AddEntryAsync(DailyEntry entry);

        Task<Optional<DailyEntry>> GetDailyEntry(int id);
    }
}
