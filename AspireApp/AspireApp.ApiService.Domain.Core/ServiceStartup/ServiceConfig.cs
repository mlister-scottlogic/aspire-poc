using AspireApp.ApiService.Domain.Core.Services;
using AspireApp.ApiService.Domain.Models;
using AspireApp.ApiService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApp.ApiService.Domain.Core.ServiceStartup
{
    public static class ServiceConfig
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddTransient<IDailyEntryService, DailyEntryService>();
            serviceProvider.AddTransient<
                IOutboxAdminService<DailyEntryWithId>,
                DailyEntryOutboxAdminService
            >();

            return serviceProvider;
        }
    }
}
