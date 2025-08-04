using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Messaging.Core.Services
{
    internal interface IDailyEntryEventer
    {
        Task<bool> DailyEntryChangedAsync(DailyEntryWithId entry);
    }
}
