using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Messaging
{
    internal interface IDailyEntryEventer
    {
        Task<bool> DailyEntryChangedAsync(DailyEntryWithId entry);
    }
}
