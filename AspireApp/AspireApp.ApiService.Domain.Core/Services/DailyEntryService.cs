using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Models;
using AspireApp.ApiService.Domain.Services;

namespace AspireApp.ApiService.Domain.Core.Services
{
    internal sealed class DailyEntryService : IDailyEntryService
    {
        private readonly IDailyEntryRepository _repository;
        private readonly ITransactionSaver _transactionSaver;

        public DailyEntryService(
            IDailyEntryRepository repository,
            ITransactionSaver transactionSaver
        )
        {
            _repository = repository;
            _transactionSaver = transactionSaver;
        }

        public async Task<DailyEntry> AddEntryAsync(DailyEntry entry)
        {
            // Get this from IdGenerator type interface to make testing possible
            var newId = Guid.NewGuid();
            var entryToSave = DailyEntry.PopulateId(newId, entry);

            await _repository.AddAsync(entryToSave);
            await _transactionSaver.SaveChangesAsync();

            return entryToSave;
        }

        public async Task<Optional<DailyEntry>> GetDailyEntry(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
