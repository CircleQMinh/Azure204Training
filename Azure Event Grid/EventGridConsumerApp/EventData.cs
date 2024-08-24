using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGridConsumerApp
{
    public class EventData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("data")]
        public InfoToDeliver Data { get; set; }
        [JsonProperty("eventType")]
        public string EventType { get; set; }
        [JsonProperty("dataVersion")]
        public string DataVersion { get; set; }
        [JsonProperty("metadataVersion")]
        public string MetadataVersion { get; set; }
        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; }
        [JsonProperty("topic")]
        public string Topic { get; set; }
    }

    public class InfoToDeliver
    {
        public string Message { get; set; }
        public string Name { get; set; }
    }
}
