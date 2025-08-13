using Microsoft.AspNetCore.Http;

namespace AspireApp.Tests.Performance.LoadFramework
{
    internal class HttpLoadScenario
    {
        private readonly int ConcurrentThreads;
        private readonly TimeSpan Duration;
        private readonly TimeSpan DelayBetweenCalls;

        private readonly Func<Task<HttpResponse>> TimedScenario;

        private HttpLoadScenario(
            Func<Task<HttpResponse>> timedScenario,
            int concurrentThreads,
            TimeSpan duration,
            TimeSpan delayBetweenCalls
        )
        {
            TimedScenario = timedScenario;
            ConcurrentThreads = concurrentThreads;
            Duration = duration;
            DelayBetweenCalls = delayBetweenCalls;
        }

        public static HttpLoadScenario Create(
            Func<Task<HttpResponse>> timedScenario,
            int concurrentRequests = 1,
            TimeSpan? duration = null,
            TimeSpan? delayBetweenCalls = null
        )
        {
            if (duration == null)
            {
                duration = TimeSpan.FromMinutes(1);
            }

            if (delayBetweenCalls == null)
            {
                delayBetweenCalls = TimeSpan.FromMilliseconds(100);
            }

            return new HttpLoadScenario(
                timedScenario,
                concurrentRequests,
                duration.Value,
                delayBetweenCalls.Value
            );
        }

        public async Task<int> RunAsync()
        {
            var result = await TimedScenario();

            return 0;
        }
    }
}
