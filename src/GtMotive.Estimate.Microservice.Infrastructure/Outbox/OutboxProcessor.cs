using System.Text.Json;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly IMongoCollection<OutboxMessage> _collection;
    private readonly IAppLogger<OutboxProcessor> _logger;
    private readonly IServiceProvider _serviceProvider;

    public OutboxProcessor(
        IServiceProvider serviceProvider,
        MongoService mongoService,
        IOptions<MongoDbSettings> options,
        IAppLogger<OutboxProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
        _collection = database.GetCollection<OutboxMessage>("Outbox");
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessMessages(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox messages.");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _logger.LogInformation("Outbox Processor is stopping.");
    }

    private async Task ProcessMessages(CancellationToken stoppingToken)
    {
        var filter = Builders<OutboxMessage>.Filter.Eq(m => m.ProcessedOnUtc, null);
        var messages = await _collection.Find(filter).Limit(20).ToListAsync(stoppingToken);

        foreach (var message in messages)
            try
            {
                var type = Type.GetType(message.Type);
                if (type == null) throw new InvalidOperationException($"Could not load type {message.Type}");

                var content = JsonSerializer.Deserialize(message.Content, type);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var busFactory = scope.ServiceProvider.GetRequiredService<IBusFactory>();
                    var bus = busFactory.GetClient(type);
                    await bus.Send(content!);
                }

                var update = Builders<OutboxMessage>.Update
                    .Set(m => m.ProcessedOnUtc, DateTime.UtcNow)
                    .Set(m => m.Error, null);
                await _collection.UpdateOneAsync(m => m.Id == message.Id, update, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing outbox message {message.Id}");
                var update = Builders<OutboxMessage>.Update.Set(m => m.Error, ex.Message);
                await _collection.UpdateOneAsync(m => m.Id == message.Id, update, cancellationToken: stoppingToken);
            }
    }
}