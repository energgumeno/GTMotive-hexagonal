using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

public class MongoRentVehicleAdapter : IRentVehiclePort
{
    private readonly IMongoCollection<RentInformation> _collection;

    public MongoRentVehicleAdapter(MongoService mongoService)
    {
        var database = mongoService.MongoClient.GetDatabase("GtMotiveEstimate");
        _collection = database.GetCollection<RentInformation>("VehicleRents");
    }

    public async Task<(List<RentInformation?>, int)> GetVehiclesRent(int pageIndex, int pageSize)
    {
        var totalCount = (int)await _collection.CountDocumentsAsync(FilterDefinition<RentInformation>.Empty);
        var rents = await _collection.Find(FilterDefinition<RentInformation>.Empty)
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (rents.Cast<RentInformation?>().ToList(), totalCount);
    }

    public async Task<RentInformation?> GetVehicleRent(Guid vehicleId)
    {
        return await _collection.Find(r => r.VehicleId == vehicleId).FirstOrDefaultAsync();
    }

    public async Task<RentInformation?> GetVehicleRent(string email)
    {
        return await _collection.Find(r => r.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Guid?> AddVehicleRent(RentInformation rentInformation)
    {
        await _collection.InsertOneAsync(rentInformation);
        return rentInformation.Id;
    }

    public async Task<Guid?> UpdateVehicleRent(RentInformation rentInformation)
    {
        var result = await _collection.ReplaceOneAsync(r => r.Id == rentInformation.Id, rentInformation);
        return result.ModifiedCount > 0 ? rentInformation.Id : null;
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
