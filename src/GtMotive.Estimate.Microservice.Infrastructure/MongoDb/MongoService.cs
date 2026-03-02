using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

public class MongoService
{
    public MongoService(IOptions<MongoDbSettings> options)
    {
        MongoClient = new MongoClient(options.Value.ConnectionString);

        RegisterBsonClasses();
    }

    public MongoClient MongoClient { get; }

    private static void RegisterBsonClasses()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Vehicle)))
        {
            BsonClassMap.RegisterClassMap<Vehicle>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.Id);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(RentInformation)))
        {
            BsonClassMap.RegisterClassMap<RentInformation>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.Id);
            });
        }
    }
}