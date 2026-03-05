using System.Linq.Expressions;
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



    public async Task<Vehicle?> GetVehicle(Expression<Func<Vehicle, bool>> filter)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<List<Vehicle>> GetVehicles(Expression<Func<Vehicle, bool>> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }
    
    public async  Task<(List<Vehicle>, int)> GetVehicles(Expression<Func<Vehicle, bool>> filter, int pageIndex, int pageSize)
    {
        var vehicles = await _collection.Find(FilterDefinition<Vehicle>.Empty)
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();
        
        return (vehicles, vehicles.Count); 
    }

    


    public async Task AddVehicle(Vehicle vehicle)
    {
        await _collection.InsertOneAsync(vehicle);
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