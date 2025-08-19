using AspireApp.ApiService.Contracts;
using Reqnroll;

namespace AspireApp.Tests.Features.StepDefinitions
{
    public static class ScenarioContextExtensions
    {
        public static void SetRequest(this ScenarioContext context, DailyEntry? request)
        {
            context["request"] = request;
        }

        public static DailyEntry GetRequest(this ScenarioContext context)
        {
            return context.Get<DailyEntry>("request");
        }

        public static void SetHttpClient(this ScenarioContext context, HttpClient client)
        {
            context["httpclient"] = client;
        }

        public static HttpClient GetHttpClient(this ScenarioContext context)
        {
            return context.Get<HttpClient>("httpclient");
        }

        public static void SetResponse(this ScenarioContext context, HttpResponseMessage? response)
        {
            context["response"] = response;
        }

        public static HttpResponseMessage GetResponse(this ScenarioContext context)
        {
            return context.Get<HttpResponseMessage>("response");
        }

        public static void SetEntryId(this ScenarioContext context, Guid? id)
        {
            context["entryid"] = id;
        }

        public static Guid? GetEntryId(this ScenarioContext context)
        {
            return context.Get<Guid?>("entryid");
        }
    }
}
