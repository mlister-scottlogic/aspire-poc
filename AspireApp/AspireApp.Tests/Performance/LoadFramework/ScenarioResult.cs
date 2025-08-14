namespace AspireApp.Tests.Performance.LoadFramework
{
    public class ScenarioResult
    {
        private readonly TimeSpan _totalDuration;
        private readonly int _concurrentRequests;

        private readonly List<TimedResult> _orderedResults;

        public ScenarioResult(
            TimeSpan totalDuration,
            int concurrentRequests,
            params IReadOnlyCollection<TimedResult>[] individualResults
        )
        {
            _totalDuration = totalDuration;
            _concurrentRequests = concurrentRequests;
            _orderedResults = individualResults
                .SelectMany(r => r)
                .OrderBy(r => r.Duration)
                .ToList();
        }

        public double FailureRate =>
            _orderedResults.Count(r => !r.Success) / (double)_orderedResults.Count;
        public double Average => Math.Round(_orderedResults.Average(r => r.Duration), 2);

        public double Min => _orderedResults.First().Duration;
        public double Max => _orderedResults.Last().Duration;

        public double Median => GetPercentile(50);
        public double Top95 => GetPercentile(95);

        public double GetPercentile(int percentile) =>
            _orderedResults[(int)(_orderedResults.Count * ((decimal)percentile / 100))].Duration;

        public override string? ToString()
        {
            return $"Test took {Math.Round(_totalDuration.TotalSeconds, 0)} seconds to send {_orderedResults.Count} requests with these happening in {_concurrentRequests} parallel streams."
                + $"\n Failure rate: {FailureRate}%"
                + $"\n Average: {Average}ms"
                + $"\n Median: {Median}ms"
                + $"\n 95th percentile: {Top95}ms"
                + $"\n 99th percentile: {GetPercentile(99)}ms";
        }
    }
}
