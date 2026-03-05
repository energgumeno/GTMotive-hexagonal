using System.Text.Json;
using Azure.Messaging.ServiceBus;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned.Commands;
using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.PU.Handlers;

public class RentVehicleReturnedMessageHandler(
    IUseCase<ProcessRentReturnedCommand> processRentReturnedUseCase,
    ILogger<RentVehicleReturnedMessageHandler> logger) : IMessageHandler
{
    private static readonly JsonSerializerOptions SerializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string MessageType => nameof(RentVehicleReturnedEvent);

    public async Task Handle(string body, ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        var returnedEvent = JsonSerializer.Deserialize<RentVehicleReturnedEvent>(body, SerializationOptions);
        if (returnedEvent != null)
            await processRentReturnedUseCase.Execute(new ProcessRentReturnedCommand(returnedEvent));
        else
            logger.LogWarning("Deserialization failed for {MessageType}. Body: {Body}", MessageType, body);
    }
}