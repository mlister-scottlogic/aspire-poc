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

        public async Task<DailyEntryWithId> AddEntryAsync(DailyEntry entry)
        {
            var result = await _repository.AddAsync(entry);
            await _transactionSaver.SaveChangesAsync();

            return result;
        }

        public async Task<Optional<DailyEntryWithId>> GetDailyEntry(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
