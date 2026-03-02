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
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = new ServiceBusClient(settings.Value.ConnectionString);
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

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        _logger.LogInformation("Received message: {Body}", body);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var jsonDocument = JsonDocument.Parse(body);
            
            // Determinar tipo de mensaje basado en estructura
            if (jsonDocument.RootElement.TryGetProperty("RentInformation", out _))
            {
                if (body.Contains("RentVehicleReturnedEvent"))
                {
                    var returnedEvent = JsonSerializer.Deserialize<RentVehicleReturnedEvent>(body);
                    if (returnedEvent != null)
                    {
                        var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<ProcessRentReturnedCommand>>();
                        await useCase.Execute(new ProcessRentReturnedCommand(returnedEvent));
                    }
                }
                else
                {
                    var createdEvent = JsonSerializer.Deserialize<RentVehicleCreatedEvent>(body);
                    if (createdEvent != null)
                    {
                        var useCase = scope.ServiceProvider.GetRequiredService<IUseCase<ProcessRentCreatedCommand>>();
                        await useCase.Execute(new ProcessRentCreatedCommand(createdEvent));
                    }
                }
            }
            else if (body.Contains("VehicleCreatedEvent"))
            {
                _logger.LogInformation("VehicleCreatedEvent received, skipping as requested.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message body: {Body}", body);
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
        await _client.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
