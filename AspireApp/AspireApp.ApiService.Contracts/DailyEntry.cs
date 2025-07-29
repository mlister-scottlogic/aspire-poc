using System.ComponentModel.DataAnnotations;
using AspireApp.ApiService.Domain.Enums;

namespace AspireApp.ApiService.Contracts
{
    public class DailyEntry
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateOnly? Date { get; set; }

        [Required]
        public decimal? Distance { get; set; }

        public DistanceUnit? DistanceUnit { get; set; }
    }
}
