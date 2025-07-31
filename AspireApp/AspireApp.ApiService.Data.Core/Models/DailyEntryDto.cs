namespace AspireApp.ApiService.Data.Core.Models
{
    public class DailyEntryDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateOnly Date { get; set; }

        public decimal Distance { get; set; }

        public required string DistanceUnit { get; set; }
    }
}
