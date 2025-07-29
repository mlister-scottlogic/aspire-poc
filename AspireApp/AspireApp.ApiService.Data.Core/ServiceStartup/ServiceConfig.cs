using AspireApp.ApiService.Data.Core.Repositories;
using AspireApp.ApiService.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApp.ApiService.Data.Core.ServiceStartup
{
    public static class ServiceConfig
    {
        public static IServiceCollection RegisterData(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddSingleton<IDailyEntryRepository, DailyEntryRepository>();

            return serviceProvider;
        }
    }
}
