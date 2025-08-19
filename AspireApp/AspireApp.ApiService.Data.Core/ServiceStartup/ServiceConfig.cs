using AspireApp.ApiService.Data.Core.Repositories;
using AspireApp.ApiService.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApp.ApiService.Data.Core.ServiceStartup
{
    public static class ServiceConfig
    {
        public static IServiceCollection RegisterData(
            this IServiceCollection serviceCollection,
            IConfigurationManager configuration
        )
        {
            serviceCollection.AddTransient<IDailyEntryRepository, DailyEntryRepository>();
            serviceCollection.AddTransient<
                IDailyEntryOutboxRepository,
                DailyEntryOutboxRepository
            >();

            serviceCollection.AddTransient<ITransactionSaver, TransactionSaver>();
            serviceCollection.AddDbContextPool<EntryContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("postgresdb"))
                    .UseSnakeCaseNamingConvention()
            );
            return serviceCollection;
        }

        public static IServiceProvider StartupData(
            this IServiceProvider serviceProvider,
            bool isDevelopment
        )
        {
            if (isDevelopment)
            {
                using var scope = serviceProvider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<EntryContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            return serviceProvider;
        }
    }
}
