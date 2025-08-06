using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Core.Models;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Enums;
using AspireApp.ApiService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.ApiService.Data.Core.Repositories
{
    internal sealed class DailyEntryOutboxRepository : IDailyEntryOutboxRepository
    {
        private const int MAX_PROCESSING_ATTEMPTS = 5;
        private const int MAX_MESSAGES_TO_PROCESS = 100;

        private readonly EntryContext _entryContext;

        public DailyEntryOutboxRepository(EntryContext entryContext)
        {
            _entryContext = entryContext;
        }

        public async Task AddAsync(DailyEntryWithId entry)
        {
            var entity = MapToEntity(entry);

            var result = await _entryContext.DailyEntryOutboxMessages.AddAsync(entity);
        }

        public async Task<IReadOnlyList<OutboxDailyEntry>> GetMessagesToProcessAsync()
        {
            var results = await _entryContext
                .DailyEntryOutboxMessages.Where(m =>
                    m.Processed == false && m.ProcessingAttempts < MAX_PROCESSING_ATTEMPTS
                )
                .OrderBy(m => m.AddedOn)
                // Limit messages to stop excessive memory usage - this could be if the
                // scheduled job stops and messages build up for example
                .Take(MAX_MESSAGES_TO_PROCESS)
                .ToListAsync();

            return results.Select(MapToOutboxModel).ToList();
        }

        public async Task MessageSuccessfullySentAsync(OutboxDailyEntry dailyEntry)
        {
            await DeleteMessageAsync(dailyEntry.Id);
        }

        public async Task MessageFailedToSendAsync(OutboxDailyEntry dailyEntry)
        {
            var result = await _entryContext.DailyEntryOutboxMessages.FindAsync(dailyEntry.Id);

            if (result != null)
            {
                result.ProcessingAttempts += 1;
                _entryContext.DailyEntryOutboxMessages.Update(result);
                await _entryContext.SaveChangesAsync();
            }

            // Log this has already been deleted? Just a warning really
        }

        public async Task<IReadOnlyCollection<OutboxDailyEntry>> GetFailedMessagesAsync()
        {
            var results = await _entryContext
                .DailyEntryOutboxMessages.Where(m =>
                    m.ProcessingAttempts >= MAX_PROCESSING_ATTEMPTS
                )
                .OrderBy(m => m.AddedOn)
                // Limit messages to stop excessive memory usage - this could be if the
                // scheduled job stops and messages build up for example
                .Take(MAX_MESSAGES_TO_PROCESS)
                .ToListAsync();

            return results.Select(MapToOutboxModel).ToList();
        }

        public async Task DeleteMessageAsync(int id)
        {
            var result = await _entryContext.DailyEntryOutboxMessages.FindAsync(id);

            if (result != null)
            {
                _entryContext.DailyEntryOutboxMessages.Remove(result);
                await _entryContext.SaveChangesAsync();
            }
        }

        public async Task<Optional<OutboxDailyEntry>> GetMessageAsync(int id)
        {
            var result = await _entryContext.DailyEntryOutboxMessages.FindAsync(id);

            if (result == null)
            {
                return Optional<OutboxDailyEntry>.None();
            }

            return Optional<OutboxDailyEntry>.Some(MapToOutboxModel(result));
        }

        private OutboxDailyEntry MapToOutboxModel(DailyEntryOutboxEntity entity)
        {
            return new OutboxDailyEntry()
            {
                Id = entity.Id,
                ProcessingAttempts = entity.ProcessingAttempts,
                Entry = new DailyEntryWithId()
                {
                    Id = entity.EntryId,
                    Date = entity.Date,
                    Title = entity.Title,
                    Description = entity.Description,
                    Distance = entity.Distance,
                    DistanceUnit = Enum.Parse<DistanceUnit>(entity.DistanceUnit),
                },
            };
        }

        private static DailyEntryOutboxEntity MapToEntity(DailyEntryWithId entry)
        {
            return new DailyEntryOutboxEntity()
            {
                //Id = set by entity framework
                AddedOn = DateTime.UtcNow,
                Processed = false,
                ProcessingAttempts = 0,

                EntryId = entry.Id!.Value,
                Title = entry.Title,
                Description = entry.Description,
                Date = entry.Date,
                Distance = entry.Distance,
                DistanceUnit = entry.DistanceUnit.ToString(),
            };
        }
    }
}
