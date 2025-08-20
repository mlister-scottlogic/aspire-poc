using Dapper;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace AspireApp.Tests.EndToEnd.Database
{
    public class DailyEntryRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public DailyEntryRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<DailyEntryEntity?> GetDailyEntryByIdAsync(Guid id)
        {
            await using var connection = await _dataSource.OpenConnectionAsync();

            return await connection.QuerySingleOrDefaultAsync<DailyEntryEntity>(
                $"SELECT * FROM apiservice.daily_entries WHERE id = '{id}'"
            );
        }
    }

    public class DailyEntryEntity
    {
        [Key]
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Distance { get; set; }

        public required string DistanceUnit { get; set; }
    }
}
