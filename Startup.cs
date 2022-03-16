using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ServiceBus
{
    class Startup
    {
        static async Task Main(string[] args)
        {
            var config = BuildConfig();

            var connectionString = config["ServiceBusConnectionString"];
            var queueName = config["ServiceBusQueue"];
            var topicName = config["ServieBusTopic"];
            var subscriptionName = config["ServieBusTopicSubscriptionName"];

            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(queueName);
            
            // Queue
            var queue = new QueueManager(client, queueName);

            await queue.PublishMessageAsync(
                JsonConvert.SerializeObject(new Order {
                    OrderID = Guid.NewGuid(),
                    ProductName = "RTX 3090",
                    Quantity = 1,
                    Price = 150000
                })
            );

            await queue.ConsumeMessageAsync();

            // Topic
            var topic = new TopicManager(client, topicName, subscriptionName);

            await topic.PublishMessageAsync(
                JsonConvert.SerializeObject(new Order {
                    OrderID = Guid.NewGuid(),
                    ProductName = "RTX 3090",
                    Quantity = 1,
                    Price = 150000
                })
            );

            await topic.ConsumeMessageAsync();

            Console.WriteLine("Main completed");
            Console.ReadKey();
        }

        static IConfiguration BuildConfig()
        {
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("./appsettings.json", optional: false)
                                    .Build();

            return configuration;
        }
    }
}
