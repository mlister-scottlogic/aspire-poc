using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Core.Models;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Enums;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Core.Repositories
{
    internal sealed class DailyEntryRepository : IDailyEntryRepository
    {
        private readonly EntryContext _entryContext;

        public DailyEntryRepository(EntryContext entryContext)
        {
            _entryContext = entryContext;
        }

        public async Task<DailyEntryWithId> AddAsync(DailyEntry entry)
        {
            // Get this from IdGenerator type interface to make testing possible
            var newId = Guid.NewGuid();
            var entryToSave = DailyEntryWithId.PopulateId(newId, entry);

            var entity = MapToEntity(entryToSave);

            var result = await _entryContext.DailyEntries.AddAsync(entity);

            return MapToDomain(result.Entity);
        }

        public async Task<Optional<DailyEntryWithId>> GetByIdAsync(Guid id)
        {
            var result = await _entryContext.DailyEntries.FindAsync(id);

            if (result is not null)
            {
                return Optional<DailyEntryWithId>.Some(MapToDomain(result));
            }

            return Optional<DailyEntryWithId>.None();
        }

        private static DailyEntryEntity MapToEntity(DailyEntryWithId entry)
        {
            return new DailyEntryEntity()
            {
                Id = entry.Id!.Value,
                Title = entry.Title,
                Description = entry.Description,
                Date = entry.Date,
                Distance = entry.Distance,
                DistanceUnit = entry.DistanceUnit.ToString(),
            };
        }

        private static DailyEntryWithId MapToDomain(DailyEntryEntity entry)
        {
            return new DailyEntryWithId()
            {
                Id = entry.Id,
                Title = entry.Title,
                Description = entry.Description,
                Date = entry.Date,
                Distance = entry.Distance,
                DistanceUnit = Enum.Parse<DistanceUnit>(entry.DistanceUnit),
            };
        }
    }
}
