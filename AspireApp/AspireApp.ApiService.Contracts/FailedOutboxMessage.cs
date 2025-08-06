namespace AspireApp.ApiService.Contracts
{
    public sealed class FailedOutboxMessage
    {
        public required int Id { get; set; }
        public required int ProcessingAttempts { get; set; }

        public required Guid LinkedId { get; set; }
    }
}
