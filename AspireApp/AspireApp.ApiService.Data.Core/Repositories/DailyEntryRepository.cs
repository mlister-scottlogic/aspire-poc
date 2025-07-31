using AspireApp.ApiService.Common;
using AspireApp.ApiService.Data.Core.Models;
using AspireApp.ApiService.Data.Repositories;
using AspireApp.ApiService.Domain.Enums;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Data.Core.Repositories
{
    internal sealed class DailyEntryRepository : IDailyEntryRepository
    {
        private readonly Dictionary<int, DailyEntryDto> _store;

        private int _currentId;

        public DailyEntryRepository()
        {
            _store = [];
        }

        public async Task<DailyEntry> AddAsync(DailyEntry entry)
        {
            await Task.Delay(10);

            var id = Interlocked.Increment(ref _currentId);

            if (_store.ContainsKey(id))
            {
                throw new InvalidOperationException("Cannot add an id twice");
            }

            var dto = MapToDto(id, entry);
            _store.Add(id, dto);

            return MapToDomain(dto);
        }

        public async Task<Optional<DailyEntry>> GetByIdAsync(int id)
        {
            await Task.Delay(10);

            if (_store.TryGetValue(id, out var dto))
            {
                return Optional<DailyEntry>.Some(MapToDomain(dto));
            }

            return Optional<DailyEntry>.None();
        }

        private static DailyEntryDto MapToDto(int id, DailyEntry entry)
        {
            return new DailyEntryDto()
            {
                Id = id,
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
