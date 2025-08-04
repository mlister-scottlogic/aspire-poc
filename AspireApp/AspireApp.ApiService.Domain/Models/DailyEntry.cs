using AspireApp.ApiService.Domain.Enums;

namespace AspireApp.ApiService.Domain.Models
{
    public class DailyEntry
    {
        public required string Title { get; init; }

        public string? Description { get; init; }

        public DateOnly Date { get; init; }

        public required decimal Distance { get; init; }

        public DistanceUnit DistanceUnit { get; init; }
    }

    public class DailyEntryWithId : DailyEntry
    {
        public static DailyEntryWithId PopulateId(Guid id, DailyEntry dailyEntry)
        {
            return new()
            {
                Id = id,
                Title = dailyEntry.Title,
                Description = dailyEntry.Description,
                Date = dailyEntry.Date,
                Distance = dailyEntry.Distance,
                DistanceUnit = dailyEntry.DistanceUnit,
            };
        }

        public Guid? Id { get; init; }
    }
}
