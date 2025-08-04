using AspireApp.ApiService.Messaging.Core.Jobs;
using AspireApp.ApiService.Messaging.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApp.ApiService.Messaging.Core.ServiceStartup
{
    public static class ServiceConfig
    {
        public static IServiceCollection RegisterMessaging(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddTransient<IDailyEntryMessagingService, DailyEntryMessagingService>();
            serviceProvider.AddTransient<IDailyEntryEventer, DailyEntryEventer>();

            return serviceProvider;
        }

        public static IServiceProvider StartupMessaging(this IServiceProvider serviceProvider)
        {
            return serviceProvider;
        }
    }
}
