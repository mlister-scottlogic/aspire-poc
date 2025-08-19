using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspireApp.Tests.EndToEnd.Database
{
    public class DailyEntryRepository
    {
        private readonly EntryContext _entryContext;

        public DailyEntryRepository(EntryContext entryContext)
        {
            _entryContext = entryContext;
        }

        public async Task<DailyEntryEntity?> GetDailyEntryById(Guid id)
        {
            return await _entryContext.DailyEntries.FindAsync(id);
        }
    }

    [Table("DailyEntries", Schema = "apiservice")]
    public class DailyEntryEntity
    {
        [Key]
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateOnly Date { get; set; }

        public decimal Distance { get; set; }

        public required string DistanceUnit { get; set; }
    }
}
