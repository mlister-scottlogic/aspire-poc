namespace AspireApp.Tests.Performance.LoadFramework
{
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
}
