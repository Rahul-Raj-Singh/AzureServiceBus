
using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace ServiceBus
{
    public class TopicManager
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly ServiceBusReceiver _receiver;

        public TopicManager(ServiceBusClient client, string topicName, string subscriptionName)
        {
            _client = client;
            _sender = client.CreateSender(topicName);
            _receiver = client.CreateReceiver(topicName, subscriptionName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock });
        }

        public async Task PublishMessageAsync(string body)
        {
            var message = new ServiceBusMessage(body);
            message.ContentType = "application/json";
            message.Subject = "Urgent";

            await _sender.SendMessageAsync(message);

            Console.WriteLine("Message published :)");
            
        }

        public async Task ConsumeMessageAsync()
        {
            var message = await _receiver.ReceiveMessageAsync(); // Receive message with LOCK duration of 30s

            Console.WriteLine($"Message is locked until: {message.LockedUntil.AddMinutes(330)}");
            Console.WriteLine(message.Body.ToString());

            await _receiver.CompleteMessageAsync(message);
        }

    }
}