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
        var ret= await _collection.Find(v => v.Id == vehicleId).FirstOrDefaultAsync();
        return ret?.Id == vehicleId ? ret : null;
    }

    public async Task<Vehicle?> GetVehicle(Vehicle vehicle)
    {
        var ret= await _collection.Find(v => v.LicensePlate == vehicle.LicensePlate || v.FrameId == vehicle.FrameId).FirstOrDefaultAsync();
        return ret?.LicensePlate == vehicle.LicensePlate || ret?.FrameId == vehicle.FrameId ? ret : null;
    }

    public async Task<Guid> AddVehicle(Vehicle vehicle)
    {
        await _collection.InsertOneAsync(vehicle);
        return vehicle.Id;
    }

    public Task<int> Save()
    {
        return Task.FromResult(1);
    }

    public void Dispose()
    {
        
        GC.SuppressFinalize(this);
    }
}
