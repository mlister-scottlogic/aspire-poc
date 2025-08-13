using System.Diagnostics;

namespace AspireApp.Tests.Performance.LoadFramework
{
    internal class HttpLoadScenario
    {
        private readonly int ConcurrentThreads;
        private readonly TimeSpan Duration;
        private readonly TimeSpan DelayBetweenCalls;

        private readonly Func<Task<HttpResponseMessage>> TimedScenario;

        private HttpLoadScenario(
            Func<Task<HttpResponseMessage>> timedScenario,
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
            Func<Task<HttpResponseMessage>> timedScenario,
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

        public async Task<ScenarioResult> RunAsync()
        {
            var result = await CreateSingleThread();

            var scenarioResult = new ScenarioResult(result);

            return scenarioResult;
        }

        private async Task<IReadOnlyCollection<TimedResult>> CreateSingleThread()
        {
            var results = new List<TimedResult>(128);

            var overallTimer = new Stopwatch();
            overallTimer.Start();

            while (overallTimer.Elapsed < Duration)
            {
                var individualTimer = Stopwatch.StartNew();
                var result = await TimedScenario();
                individualTimer.Stop();

                results.Add(
                    new TimedResult()
                    {
                        Duration = individualTimer.Elapsed.TotalMilliseconds,
                        HttpStatus = result.StatusCode,
                        Success = result.IsSuccessStatusCode,
                    }
                );

                await Task.Delay(DelayBetweenCalls);
            }

            return results;
        }
    }

    public class ScenarioResult
    {
        private readonly IReadOnlyCollection<TimedResult> _individualResults;

        public ScenarioResult(params IReadOnlyCollection<TimedResult>[] individualResults)
        {
            _individualResults = individualResults.SelectMany(r => r).ToList();
        }

        public int Failures => _individualResults.Count(r => !r.Success);
        public double Average => _individualResults.Average(r => r.Duration);
    }

    public struct TimedResult
    {
        public required bool Success { get; set; }
        public required double Duration { get; set; }
        public required HttpStatusCode HttpStatus { get; set; }
    }
}
