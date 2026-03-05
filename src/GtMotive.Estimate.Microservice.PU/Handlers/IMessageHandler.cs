using Azure.Messaging.ServiceBus;

namespace GtMotive.Estimate.Microservice.PU.Handlers;

public interface IMessageHandler
{
    string MessageType { get; }
    Task Handle(string body, ServiceBusReceivedMessage message, CancellationToken cancellationToken);
}
