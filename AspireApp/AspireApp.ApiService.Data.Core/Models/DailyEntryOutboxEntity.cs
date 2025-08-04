using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspireApp.ApiService.Data.Core.Models
{
    [Table("DailyEntryMessages", Schema = "outbox")]
    internal class DailyEntryOutboxEntity : IOutboxEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime AddedOn { get; set; }

        public bool Processed { get; set; }

        public int ProcessingAttempts { get; set; }

        public Guid EntryId { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateOnly Date { get; set; }

        public decimal Distance { get; set; }

        public required string DistanceUnit { get; set; }
    }
}
