using AspireApp.ApiService.Messaging.Core.Jobs;
using AspireApp.ApiService.Messaging.Core.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApp.ApiService.Messaging.Core.ServiceStartup
{
    public static class ServiceConfig
    {
        public static IServiceCollection RegisterMessaging(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddTransient<IDailyEntryJob, DailyEntryJob>();
            serviceProvider.AddTransient<IDailyEntryEventer, DailyEntryEventer>();

            return serviceProvider;
        }

        public static IServiceProvider StartupMessaging(this IServiceProvider serviceProvider)
        {
            RecurringJob.AddOrUpdate(
                "daily_entries_messaging",
                (DailyEntryJob job) => job.ProcessDailyEntriesAsync(),
                // Every 5 seconds
                "*/5 * * * * *"
            );

            return serviceProvider;
        }
    }
}
