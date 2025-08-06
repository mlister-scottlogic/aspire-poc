using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Models;
using AspireApp.ApiService.Domain.Services;
using AspireApp.ApiService.Messaging;

namespace AspireApp.ApiService.Domain.Core.Services
{
    internal sealed class DailyEntryOutboxAdminService : IOutboxAdminService<DailyEntryWithId>
    {
        private readonly IDailyEntryOutboxRepository _repository;
        private readonly IDailyEntryMessagingService _service;

        public DailyEntryOutboxAdminService(
            IDailyEntryOutboxRepository repository,
            IDailyEntryMessagingService service
        )
        {
            _repository = repository;
            _service = service;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var result = await _repository.GetMessageAsync(id);

            return await result.MatchAsync(
                async (e) =>
                {
                    await _repository.DeleteMessageAsync(id);
                    return true;
                },
                () => Task.FromResult(false)
            );
        }

        public async Task<Optional<FailedOutboxMessage>> GetFailedMessageAsync(int id)
        {
            var result = await _repository.GetMessageAsync(id);

            return result.Match(
                (e) => Optional<FailedOutboxMessage>.Some(ToDomain(e)),
                Optional<FailedOutboxMessage>.None
            );
        }

        public async Task<IReadOnlyCollection<FailedOutboxMessage>> GetFailedMessagesAsync()
        {
            var results = await _repository.GetFailedMessagesAsync();

            return results.Select(ToDomain).ToList();
        }

        public async Task<bool> RetryMessageAsync(int id)
        {
            var result = await _repository.GetMessageAsync(id);

            return await result.MatchAsync(
                async (m) =>
                {
                    await _service.ProcessDailyEntryMessageAsync(m);
                    return true;
                },
                () => Task.FromResult(false)
            );
        }

        private FailedOutboxMessage ToDomain(OutboxDailyEntry dailyEntry)
        {
            return new FailedOutboxMessage()
            {
                Id = dailyEntry.Id,
                ProcessingAttempts = dailyEntry.ProcessingAttempts,
                LinkedId = dailyEntry.Entry.Id!.Value,
            };
        }
    }
}
