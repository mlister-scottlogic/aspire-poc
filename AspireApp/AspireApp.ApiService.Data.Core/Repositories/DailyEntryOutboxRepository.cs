using AspireApp.ApiService.Data.Core.Models;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Core.Repositories
{
    internal sealed class DailyEntryOutboxRepository : IDailyEntryOutboxRepository
    {
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

        private static DailyEntryOutboxEntity MapToEntity(DailyEntryWithId entry)
        {
            return new DailyEntryOutboxEntity()
            {
                //Id = set by entity framework
                //AddedOn = set by entity framework
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
