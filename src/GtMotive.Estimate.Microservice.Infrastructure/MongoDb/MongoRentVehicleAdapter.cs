using System.Linq.Expressions;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

public class MongoRentVehicleAdapter : IRentVehiclePort
{
    private readonly IMongoCollection<RentInformation> _collection;

    public MongoRentVehicleAdapter(MongoService mongoService, IOptions<MongoDbSettings> options)
    {
        var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
        _collection = database.GetCollection<RentInformation>("VehicleRents");
    }



    public async   Task<RentInformation?> GetVehicleRent(Expression<Func<RentInformation, bool>> filter)
    {
        return  await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<(List<RentInformation>, int)> GetVehiclesRent(int pageIndex, int pageSize)
    {
        return await GetVehiclesRent(r => true, pageIndex, pageSize);
    }

    public async   Task<List<RentInformation>> GetVehiclesRent(Expression<Func<RentInformation, bool>> filter)
    {
        return  await _collection.Find(filter).ToListAsync()??[];
    }
    public async  Task<(List<RentInformation>, int)> GetVehiclesRent(Expression<Func<RentInformation, bool>> filter, int pageIndex, int pageSize)
    {
        
        var rents = await _collection.Find(filter)
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (rents, rents.Count); 
        
    }

    public async Task AddVehicleRent(RentInformation rentInformation)
    {
        await _collection.InsertOneAsync(rentInformation);
    }

    public async Task UpdateVehicleRent(RentInformation rentInformation)
    {
        await _collection.ReplaceOneAsync(r => r.Id == rentInformation.Id, rentInformation);
       
    }

    public Task<int> Save()
    {
        return Task.FromResult(1);
    }
    
}