using System.Text.Json;
using Azure.Messaging.ServiceBus;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Commands;
using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.PU.Handlers;

public class RentVehicleCreatedMessageHandler(
    IUseCase<ProcessRentCreatedCommand> processRentCreatedUseCase,
    ILogger<RentVehicleCreatedMessageHandler> logger) : IMessageHandler
{
    private static readonly JsonSerializerOptions SerializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string MessageType => nameof(RentVehicleCreatedEvent);

    public async Task Handle(string body, ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        var createdEvent = JsonSerializer.Deserialize<RentVehicleCreatedEvent>(body, SerializationOptions);
        if (createdEvent != null)
            await processRentCreatedUseCase.Execute(new ProcessRentCreatedCommand(createdEvent));
        else
            logger.LogWarning("Deserialization failed for {MessageType}. Body: {Body}", MessageType, body);
    }
}