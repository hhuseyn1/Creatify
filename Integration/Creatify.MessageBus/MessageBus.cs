﻿using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Creatify.MessageBus;

public class MessageBus : IMessageBus
{
    private string connectionString = "";
    public async Task PublishMessage(object message, string topic_queue_name)
    {
        await using var client = new ServiceBusClient(connectionString);

        ServiceBusSender busSender = client.CreateSender(topic_queue_name);

        var jsonMessage = JsonConvert.SerializeObject(message);
        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };
        await busSender.SendMessageAsync(serviceBusMessage);
        await client.DisposeAsync();    
    }
}
