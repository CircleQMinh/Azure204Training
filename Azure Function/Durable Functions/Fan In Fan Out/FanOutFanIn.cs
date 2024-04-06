using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Azure.Training
{
    public static class FanOutFanIn
    {
        [Function(nameof(RunOrchestrator))]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(RunOrchestrator));
            logger.LogInformation("RunOrchestrator is running");

            //called helper activity to get inputs
            var inputs = await context.CallActivityAsync<List<int>>("HelperActivity_1", 10);
            var tasks = new Task<string>[inputs.Count];
            for (int i = 0; i < inputs.Count; i++)
            {
                tasks[i] = context.CallActivityAsync<string>(
                    "F1",new FOFIInput
                    {
                        Input = inputs[i],
                        Index = i
                    });
            }
            await Task.WhenAll(tasks);

            var result =  tasks.Select(x => x.Result);
            //may call another function to process
            return string.Join(";",result);
        }

        [Function("HelperActivity_1")]
        public static List<int> HelperActivity_1(
            [ActivityTrigger] int input,
           FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("HelperActivity_1");
            
            return new List<int>(){
                input,
                input + 30,
                input + 60
            };
        }

        [Function("F1")]
        public static async Task<string> F1([ActivityTrigger] FOFIInput input, FunctionContext executionContext)
        {
            await Task.Delay(input.Input * 1000);
            return $"No-{input.Index} Complete";
        }




        [Function("FanOutFanIn_HttpStart")]
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
