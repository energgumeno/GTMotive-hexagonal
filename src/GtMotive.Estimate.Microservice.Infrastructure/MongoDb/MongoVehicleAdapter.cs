using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

public class MongoVehicleAdapter : IVehiclePort
{
    private readonly IMongoCollection<Vehicle> _collection;

    public MongoVehicleAdapter(MongoService mongoService, IOptions<MongoDbSettings> options)
    {
        var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
        _collection = database.GetCollection<Vehicle>("Vehicles");
    }

    public async Task<(List<Vehicle?>, int)> GetVehicles(int pageIndex, int pageSize)
    {
        var totalCount = (int)await _collection.CountDocumentsAsync(FilterDefinition<Vehicle>.Empty);
        var vehicles = await _collection.Find(FilterDefinition<Vehicle>.Empty)
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (vehicles.Cast<Vehicle?>().ToList(), totalCount);
    }

    public async Task<Vehicle?> GetVehicle(Guid vehicleId)
    {
        return await _collection.Find(v => v.Id == vehicleId).FirstOrDefaultAsync();
    }

    public async Task<Vehicle?> GetVehicle(Vehicle vehicle)
    {
        return await _collection.Find(v => v.LicensePlate == vehicle.LicensePlate || v.FrameId == vehicle.FrameId).FirstOrDefaultAsync();
    }

    public async Task<Guid> AddVehicle(Vehicle vehicle)
    {
        await _collection.InsertOneAsync(vehicle);
        return vehicle.Id;
    }

    public Task<int> Save()
    {
        // En MongoDB, las operaciones de escritura individuales son atómicas y persisten inmediatamente.
        // Si no se usa una sesión de cliente para transacciones multi-documento, simplemente devolvemos 1.
        return Task.FromResult(1);
    }

    public void Dispose()
    {
        // No es necesario liberar recursos específicos de MongoDB aquí
        GC.SuppressFinalize(this);
    }
}
