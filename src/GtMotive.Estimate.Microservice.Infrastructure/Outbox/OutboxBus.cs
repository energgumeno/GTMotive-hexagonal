using System.Text.Json;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Outbox;

public class OutboxBus : IBus
{
    private readonly IMongoCollection<OutboxMessage> _collection;

    public OutboxBus(MongoService mongoService, IOptions<MongoDbSettings> options)
        : this(mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName))
    {
    }

    public OutboxBus(IMongoDatabase database)
    {
        _collection = database.GetCollection<OutboxMessage>("Outbox");
    }

    public async Task Send(object message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        var outboxMessage = new OutboxMessage
        {
            Type = message.GetType().AssemblyQualifiedName,
            Content = JsonSerializer.Serialize(message),
            OccurredOnUtc = DateTime.UtcNow
        };

        await _collection.InsertOneAsync(outboxMessage);
    }
}