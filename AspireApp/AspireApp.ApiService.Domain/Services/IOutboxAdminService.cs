using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Domain.Services
{
    public interface IOutboxAdminService<T>
    {
        Task<IReadOnlyCollection<FailedOutboxMessage>> GetFailedMessages();

        Task<bool> RetryMessage(int id);

        Task DeleteMessage(int id);
    }
}
