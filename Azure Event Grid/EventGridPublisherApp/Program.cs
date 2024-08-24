// See https://aka.ms/new-console-template for more information
using Azure.Messaging.EventGrid;
using Azure;
using Microsoft.Extensions.Configuration;
using EventGridPublisherApp;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);

var config = builder.Build();

// in the "Overview" section in the "Event Grid Topics" blade in Azure Portal.
// "Event Grid Topics" blade in Azure Portal.
var topicEndpoint = config["TopicEndPoint"];
var topicKey = config["TopicKey"];

Console.WriteLine(topicEndpoint);
Console.WriteLine(topicKey);

EventGridPublisherClient client = new EventGridPublisherClient(
    new Uri(topicEndpoint),
    new AzureKeyCredential(topicKey));

await TopicPublish.PublishSingleEvent(client);
//await TopicPublish.PublishMultipleEvents(client);
Console.WriteLine("Publish Event Grid events to an Event Grid Topic");