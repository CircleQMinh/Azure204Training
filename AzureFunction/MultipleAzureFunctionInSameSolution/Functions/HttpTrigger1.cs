using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AzureFunction.Training
{
    public class HttpTrigger1
    {
        private readonly ILogger<HttpTrigger1> _logger;
        private readonly IConfiguration _config;

        public HttpTrigger1(ILogger<HttpTrigger1> logger,IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [Function("HttpTrigger1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            var secret = _config.GetValue<string>("SecretFromSetting");
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions! " + secret);
        }
    }
}
