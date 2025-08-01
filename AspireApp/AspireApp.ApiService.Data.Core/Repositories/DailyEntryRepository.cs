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
            var dto = MapToDto(entry);

            var result = await _entryContext.DailyEntries.AddAsync(dto);

            await _entryContext.SaveChangesAsync();

            return MapToDomain(result.Entity);
        }

        public async Task<Optional<DailyEntry>> GetByIdAsync(int id)
        {
            var result = await _entryContext.DailyEntries.FindAsync(id);

            if (result is not null)
            {
                return Optional<DailyEntry>.Some(MapToDomain(result));
            }

            return Optional<DailyEntry>.None();
        }

        private static DailyEntryDto MapToDto(DailyEntry entry)
        {
            return new DailyEntryDto()
            {
                Title = entry.Title,
                Description = entry.Description,
                Date = entry.Date,
                Distance = entry.Distance,
                DistanceUnit = entry.DistanceUnit.ToString(),
            };
        }

        private static DailyEntry MapToDomain(DailyEntryDto entry)
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
