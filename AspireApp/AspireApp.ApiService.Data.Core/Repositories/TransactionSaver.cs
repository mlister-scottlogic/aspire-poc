using AspireApp.ApiService.Data.Repositories;

namespace AspireApp.ApiService.Data.Core.Repositories
{
    // EntryContext is scoped so will be the same instance used in every repository
    internal sealed class TransactionSaver : ITransactionSaver
    {
        private readonly EntryContext _entryContext;

        public TransactionSaver(EntryContext entryContext)
        {
            _entryContext = entryContext;
        }

        public async Task SaveChangesAsync()
        {
            await _entryContext.SaveChangesAsync();
        }
    }
}
