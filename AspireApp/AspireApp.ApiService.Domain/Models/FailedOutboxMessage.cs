namespace AspireApp.ApiService.Domain.Models
{
    public record FailedOutboxMessage
    {
        public required int Id { get; init; }
        public required int ProcessingAttempts { get; init; }

        public required Guid LinkedId { get; init; }
    }
}
