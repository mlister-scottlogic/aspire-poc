using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Models;

namespace AspireApp.ApiService.Data.Repositories
{
    public interface IDailyEntryRepository
    {
        Task<DailyEntryDto> AddAsync(DailyEntryDto entry);
        Task<Optional<DailyEntryDto>> GetByIdAsync(int id);
    }
}
