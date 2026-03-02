using System.Text.Json;
using Azure.Messaging.ServiceBus;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.ServiceBus;

public class AzureBus : IBus
{
    private readonly ServiceBusSender _sender;

    public AzureBus(ServiceBusSender sender)
    {
        _sender = sender;
    }

    public async Task Send(object message)
    {
        var messageBody = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(messageBody)
        {
            ContentType = "application/json"
        };
        serviceBusMessage.ApplicationProperties.Add("MessageType", message.GetType().Name);
        await _sender.SendMessageAsync(serviceBusMessage);
    }
}