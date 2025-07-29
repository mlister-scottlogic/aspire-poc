using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Models;
using AspireApp.ApiService.Domain.Services;

namespace AspireApp.ApiService.Domain.Core.Services
{
    internal sealed class DailyEntryService : IDailyEntryService
    {
        private readonly IDailyEntryRepository _repository;

        public DailyEntryService(IDailyEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<DailyEntry> AddEntryAsync(DailyEntry entry)
        {
            return await _repository.AddAsync(entry);
        }

        public async Task<Optional<DailyEntry>> GetDailyEntry(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
