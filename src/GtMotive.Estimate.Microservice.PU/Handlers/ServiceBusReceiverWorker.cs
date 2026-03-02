using System.Text.Json;
using Azure.Messaging.ServiceBus;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned;
using GtMotive.Estimate.Microservice.Domain.Events;
using GtMotive.Estimate.Microservice.Infrastructure.ServiceBus;
using Microsoft.Extensions.Options;

namespace GtMotive.Estimate.Microservice.PU.Handlers;

public class ServiceBusReceiverWorker : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<ServiceBusReceiverWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ServiceBusReceiverWorker(
        IOptions<ServiceBusSettings> settings,
        ILogger<ServiceBusReceiverWorker> logger,
        IServiceProvider serviceProvider,
        AzureBusFactory busFactory)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = busFactory.GetClientInstance();
        _processor = _client.CreateProcessor(settings.Value.QueueName, new ServiceBusProcessorOptions());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        await _processor.StopProcessingAsync(stoppingToken);
    }

    private static readonly JsonSerializerOptions SerializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        _logger.LogInformation("Received message: {Body}", body);

        if (!args.Message.ApplicationProperties.TryGetValue("MessageType", out var messageTypeHeader) ||
            messageTypeHeader is not string messageType)
        {
            _logger.LogWarning("Message received without MessageType header. Body: {Body}", body);
            await args.CompleteMessageAsync(args.Message);
            return;
        }

        try
        {
            using var scope = _serviceProvider.CreateScope();

            switch (messageType)
            {
                case nameof(RentVehicleReturnedEvent):
                    var returnedEvent = JsonSerializer.Deserialize<RentVehicleReturnedEvent>(body, SerializationOptions);
                    if (returnedEvent != null)
                    {
                        var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<ProcessRentReturnedCommand>>();
                        await useCase.Execute(new ProcessRentReturnedCommand(returnedEvent));
                    }
                    else
                    {
                        _logger.LogWarning("Deserialization failed for {MessageType}. Body: {Body}", messageType, body);
                    }

                    break;

                case nameof(RentVehicleCreatedEvent):
                    var createdEvent = JsonSerializer.Deserialize<RentVehicleCreatedEvent>(body, SerializationOptions);
                    if (createdEvent != null)
                    {
                        var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<ProcessRentCreatedCommand>>();
                        await useCase.Execute(new ProcessRentCreatedCommand(createdEvent));
                    }
                    else
                    {
                        _logger.LogWarning("Deserialization failed for {MessageType}. Body: {Body}", messageType, body);
                    }

                    break;

                case nameof(VehicleCreatedEvent):
                    _logger.LogInformation("VehicleCreatedEvent received, skipping as requested.");
                    break;

                default:
                    _logger.LogWarning("Unknown MessageType: {MessageType}", messageType);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message with type {MessageType} and body: {Body}", messageType, body);
        }

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error processing message");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.DisposeAsync();
        // El cliente de ServiceBus es gestionado por la factoría registrada como Singleton
        await base.StopAsync(cancellationToken);
    }
}
