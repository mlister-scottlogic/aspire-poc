namespace AspireApp.ApiService.Data.Core.Models
{
    internal interface IOutboxEntity
    {
        DateTime AddedOn { get; set; }
        Guid Id { get; set; }
        bool Processed { get; set; }
        int ProcessingAttempts { get; set; }
    }
}
