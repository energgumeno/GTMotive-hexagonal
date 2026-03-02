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

                // Forzar el uso del constructor privado mapeando los parámetros a las propiedades/campos de BSON.
                // Esto es necesario para clases sin constructor sin parámetros y con propiedades de solo lectura.
                cm.MapCreator(v => new Vehicle(v.RegistrationDate, v.FrameId, v.LicensePlate));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(RentInformation)))
        {
            BsonClassMap.RegisterClassMap<RentInformation>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(r => r.Fullname);
                cm.MapProperty(r => r.Email);
                cm.MapProperty(r => r.VehicleId);
                cm.MapProperty(r => r.Status);
                cm.MapProperty(r => r.TimeRentStart);
                cm.MapProperty(r => r.TimeRentEnd);

                cm.MapCreator(r => new RentInformation(r.Fullname, r.Email, r.TimeRentStart, r.TimeRentEnd, r.VehicleId, r.Status));
            });
        }
    }
}