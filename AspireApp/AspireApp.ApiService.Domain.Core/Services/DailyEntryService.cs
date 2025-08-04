using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Models;
using AspireApp.ApiService.Domain.Services;

namespace AspireApp.ApiService.Domain.Core.Services
{
    internal sealed class DailyEntryService : IDailyEntryService
    {
        private readonly IDailyEntryRepository _repository;
        private readonly IDailyEntryOutboxRepository _outboxRepository;
        private readonly ITransactionSaver _transactionSaver;

        public DailyEntryService(
            IDailyEntryRepository repository,
            IDailyEntryOutboxRepository outboxRepository,
            ITransactionSaver transactionSaver
        )
        {
            _repository = repository;
            _outboxRepository = outboxRepository;
            _transactionSaver = transactionSaver;
        }

        public async Task<DailyEntryWithId> AddEntryAsync(DailyEntry entry)
        {
            // Must add to general repository and the outbox before saving
            // This way a fail to write to either causes an overall failure
            var result = await _repository.AddAsync(entry);
            await _outboxRepository.AddAsync(result);
            await _transactionSaver.SaveChangesAsync();

            return result;
        }

        public async Task<Optional<DailyEntryWithId>> GetDailyEntry(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
