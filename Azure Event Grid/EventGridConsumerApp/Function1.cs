using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EventGridConsumerApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
               [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
               ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string response = string.Empty;
            BinaryData events = await BinaryData.FromStreamAsync(req.Body);

            HandleReceiveEvent(events,log);

            EventGridEvent[] eventGridEvents = EventGridEvent.ParseMany(events);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                // Handle system events
                if (eventGridEvent.TryGetSystemEventData(out object eventData))
                {
                    // Handle the subscription validation event
                    if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                    {
                        log.LogInformation($"Got SubscriptionValidation event data, validation code: {subscriptionValidationEventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                        // Do any additional validation (as required) and then return back the below response
                        var responseData = new
                        {
                            ValidationResponse = subscriptionValidationEventData.ValidationCode
                        };

                        return new OkObjectResult(responseData);
                    }
                }
            }
            return new OkObjectResult(response);
        }

        private static void HandleReceiveEvent(BinaryData events, ILogger log)
        {
            log.LogInformation($"Received events: {events}");
            try
            {
                var data = JsonConvert.DeserializeObject<List<EventData>>(events.ToString());
                var firstEvent = data.FirstOrDefault();
                var logTest = firstEvent != null ? firstEvent.Data.Message : "null";
                log.LogInformation($"Received data: {logTest}"); 
            }
            catch (Exception ex)
            {
                log.LogInformation($"Can not convert objects. {ex.Message}");
            }
            
        }
    }
}
