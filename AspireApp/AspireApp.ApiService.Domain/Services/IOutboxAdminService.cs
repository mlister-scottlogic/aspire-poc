using AspireApp.ApiService.Common;
using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Domain.Services
{
    public interface IOutboxAdminService<T>
    {
        Task<IReadOnlyCollection<FailedOutboxMessage>> GetFailedMessagesAsync();

        Task<Optional<FailedOutboxMessage>> GetFailedMessageAsync(int id);

        Task<bool> RetryMessageAsync(int id);

        Task<bool> DeleteMessageAsync(int id);
    }
}
