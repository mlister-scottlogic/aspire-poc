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

        public async Task<DailyEntry> AddAsync(DailyEntry entry)
        {
            var entity = MapToEntity(entry);

            var result = await _entryContext.DailyEntries.AddAsync(entity);

            return MapToDomain(result.Entity);
        }

        public async Task<Optional<DailyEntry>> GetByIdAsync(Guid id)
        {
            var result = await _entryContext.DailyEntries.FindAsync(id);

            if (result is not null)
            {
                return Optional<DailyEntry>.Some(MapToDomain(result));
            }

            return Optional<DailyEntry>.None();
        }

        private static DailyEntryEntity MapToEntity(DailyEntry entry)
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

        private static DailyEntry MapToDomain(DailyEntryEntity entry)
        {
            return new DailyEntry()
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
