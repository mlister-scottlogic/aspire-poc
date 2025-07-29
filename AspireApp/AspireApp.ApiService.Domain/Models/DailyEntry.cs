using AspireApp.ApiService.Domain.Enums;

namespace AspireApp.ApiService.Domain.Models
{
    public class DailyEntry
    {
        public int? Id { get; init; }

        public required string Title { get; init; }

        public string? Description { get; init; }

        public DateOnly Date { get; init; }

        public required decimal Distance { get; init; }

        public DistanceUnit DistanceUnit { get; init; }
    }
}
