using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Domain.Services
{
    public interface IOutboxAdminService<T>
    {
        Task<IReadOnlyCollection<FailedOutboxMessage>> GetFailedMessagesAsync();

        Task<FailedOutboxMessage> GetFailedMessageAsync(int id);

        Task RetryMessageAsync(int id);

        Task DeleteMessageAsync(int id);
    }
}
