using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
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
        // Se configura la representación de Guid globalmente para evitar errores de serialización.
        // Se recomienda usar el formato Standard de MongoDB.
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        if (!BsonClassMap.IsClassMapRegistered(typeof(BaseAggregate)))
        {
            BsonClassMap.RegisterClassMap<BaseAggregate>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.Id);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Vehicle)))
        {
            BsonClassMap.RegisterClassMap<Vehicle>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(v => v.RegistrationDate);
                cm.MapProperty(v => v.FrameId);
                cm.MapProperty(v => v.LicensePlate);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(RentInformation)))
        {
            BsonClassMap.RegisterClassMap<RentInformation>(cm =>
            {
                cm.AutoMap();
            });
        }
    }
}