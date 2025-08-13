namespace AspireApp.Tests.Performance.LoadFramework
{
    public struct TimedResult
    {
        public required bool Success { get; set; }
        public required double Duration { get; set; }
        public required HttpStatusCode HttpStatus { get; set; }
    }
}
