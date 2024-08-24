using Azure.Messaging.EventGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGridPublisherApp
{
    public static class TopicPublish
    {
        //Publish Event Grid events to an Event Grid Topic

        public static InfoToDeliver GetInfoToDeliver(string name)
        {
            return new InfoToDeliver() { 
                Message = $"Hi my name is {name}",
                Name = name
            };
        }
        public static async Task PublishSingleEvent(EventGridPublisherClient client)
        {
            EventGridEvent egEvent =
            new EventGridEvent(
                "This is My Event Subject",
                "This is My Event Type",
                "1.0",
                GetInfoToDeliver("Bob"));

            // Send the event
            await client.SendEventAsync(egEvent);
        }

        public static async Task PublishMultipleEvents(EventGridPublisherClient client)
        {
            List<EventGridEvent> list = new List<EventGridEvent>();
            list.Add(new EventGridEvent(
                "This is My Event Subject",
                "This is My Event Type",
                "1.0",
                GetInfoToDeliver("John")));
            list.Add(new EventGridEvent(
                "This is My Event Subject",
                "This is My Event Type",
                "1.0",
                GetInfoToDeliver("Tiny")));
            await client.SendEventsAsync(list);

        }
    }
}
