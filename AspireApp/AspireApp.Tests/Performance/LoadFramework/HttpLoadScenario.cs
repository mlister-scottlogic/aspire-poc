using System.Diagnostics;

namespace AspireApp.Tests.Performance.LoadFramework
{
    internal class HttpLoadScenario
    {
        private readonly int ConcurrentRequests;
        private readonly TimeSpan Duration;
        private readonly TimeSpan DelayBetweenCalls;

        private readonly Func<Task<HttpResponseMessage>> TimedScenario;

        private HttpLoadScenario(
            Func<Task<HttpResponseMessage>> timedScenario,
            int concurrentRequests,
            TimeSpan duration,
            TimeSpan delayBetweenCalls
        )
        {
            TimedScenario = timedScenario;
            ConcurrentRequests = concurrentRequests;
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

        public async Task<ScenarioResult> RunAsync(bool warmup = false)
        {
            if (warmup)
            {
                await CreateSingleTask(TimeSpan.FromSeconds(20));
            }

            var startTime = Stopwatch.GetTimestamp();

            var tasks = Enumerable
                .Range(0, ConcurrentRequests)
                .Select(i => CreateSingleTask(Duration));

            var results = await Task.WhenAll(tasks);

            var totalTime = Stopwatch.GetElapsedTime(startTime);

            var scenarioResult = new ScenarioResult(totalTime, ConcurrentRequests, results);

            return scenarioResult;
        }

        private async Task<IReadOnlyCollection<TimedResult>> CreateSingleTask(
            TimeSpan overallDuration
        )
        {
            var results = new List<TimedResult>(128);

            var overallTimer = new Stopwatch();
            overallTimer.Start();

            while (overallTimer.Elapsed < overallDuration)
            {
                var startTime = Stopwatch.GetTimestamp();
                var result = await TimedScenario();
                var duration = Stopwatch.GetElapsedTime(startTime);

                results.Add(
                    new TimedResult()
                    {
                        Duration = duration.TotalMilliseconds,
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

        private readonly TimeSpan _totalDuration;
        private readonly int _concurrentRequests;

        public ScenarioResult(
            TimeSpan totalDuration,
            int concurrentRequests,
            params IReadOnlyCollection<TimedResult>[] individualResults
        )
        {
            _totalDuration = totalDuration;
            _concurrentRequests = concurrentRequests;
            _individualResults = individualResults.SelectMany(r => r).ToList();
        }

        public int Failures => _individualResults.Count(r => !r.Success);
        public double Average => Math.Round(_individualResults.Average(r => r.Duration), 2);

        public override string? ToString()
        {
            return $"Test took {Math.Round(_totalDuration.TotalSeconds, 0)} seconds to send {_individualResults.Count} requests with these happening in {_concurrentRequests} parallel streams."
                + $"\n Failures: {Failures}"
                + $"\n Average: {Average}ms";
        }
    }

    public struct TimedResult
    {
        public required bool Success { get; set; }
        public required double Duration { get; set; }
        public required HttpStatusCode HttpStatus { get; set; }
    }
}
