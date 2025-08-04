using AspireApp.ApiService.Domain.Models;

namespace AspireApp.ApiService.Messaging.Core.Services
{
    internal sealed class DailyEntryEventer : IDailyEntryEventer
    {
        public Task<bool> DailyEntryChangedAsync(DailyEntryWithId entry)
        {
            throw new NotImplementedException();
        }
    }
}
