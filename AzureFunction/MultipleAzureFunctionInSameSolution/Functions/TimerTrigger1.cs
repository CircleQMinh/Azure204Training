using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MultipleAzureFunctionInSameSolution.Services;
namespace AzureFunction.Training
{
    public class TimerTrigger1
    {
        private readonly ILogger _logger;
        private readonly IMyService _myservice;

        public TimerTrigger1(ILoggerFactory loggerFactory, IMyService myservice)
        {
            _logger = loggerFactory.CreateLogger<TimerTrigger1>();
            _myservice = myservice;
        }

        [Function("TimerTrigger1")]
        public async Task Run([TimerTrigger("*/30 * * * * *")] TimerInfo myTimer) // run every 30s
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
            var secret = _myservice.IsMyServiceWorking();
            _logger.LogInformation(secret);
            string url = "http://localhost:7071/api/HttpTrigger1"; // Example URL
            await MakeGetRequest(url);
           
        }

        private async Task MakeGetRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Response Body:\n{responseBody}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to make GET request. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error occurred while making GET request: {e.Message}");
                }
            }
        }
    }
}
