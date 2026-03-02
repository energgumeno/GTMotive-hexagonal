using Azure.Messaging.ServiceBus;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace GtMotive.Estimate.Microservice.Infrastructure.ServiceBus;

public class AzureBusFactory : IBusFactory, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly string _queueName;

    public AzureBusFactory(IOptions<ServiceBusSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Value.ConnectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Value.QueueName);

        _client = new ServiceBusClient(settings.Value.ConnectionString);
        _queueName = settings.Value.QueueName;
    }

    public IBus GetClient(Type eventType)
    {
        // En esta implementación simplificada, usamos la misma cola para todos los eventos.
        // Se podría extender para usar diferentes colas/temas basados en el eventType.
        var sender = _client.CreateSender(_queueName);
        return new AzureBus(sender);
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
