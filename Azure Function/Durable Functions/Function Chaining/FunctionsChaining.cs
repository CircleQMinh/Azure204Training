using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Azure.Training
{
    public static class FunctionsChaining
    {
        [Function(nameof(RunOrchestrator))]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(FunctionsChaining));
            logger.LogInformation("RunOrchestrator running.");

            var f1 = await context.CallActivityAsync<string>(nameof(F1), "My OG Input");
            var f2 = await context.CallActivityAsync<string>(nameof(F2), f1);
            var f3 = await context.CallActivityAsync<string>(nameof(F3), f2);

            return f3;
        }

        [Function("F1")]
        public static string F1([ActivityTrigger] string input, FunctionContext executionContext)
        {
            var secretFromF1 = "F1 is here";
            return $"{input}-{secretFromF1}";
        }
        [Function("F2")]
        public static string F2([ActivityTrigger] string input, FunctionContext executionContext)
        {
            var secretFromF2 = "F2 is here";
            return $"{input}-{secretFromF2}";
        }
        [Function("F3")]
        public static string F3([ActivityTrigger] string input, FunctionContext executionContext)
        {
            var secretFromF3 = "F3 is here";
            return $"{input}-{secretFromF3}!";
        }


        [Function("FunctionsChaining_Start")]
        public static async Task<HttpResponseData> MyClientFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("MyClientFunction");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(RunOrchestrator));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
