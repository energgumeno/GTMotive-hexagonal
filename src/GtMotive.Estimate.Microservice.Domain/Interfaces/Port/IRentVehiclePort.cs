using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Port
{
    public interface IRentVehiclePort:IUnitOfWork
    {
        Task<(List<RentInformation?>,int)> GetVehiclesRent(int pageIndex, int pageSize);
        Task<RentInformation?> GetVehicleRent(Guid vehicleId);
        Task<RentInformation?> GetVehicleRent(string Email);
        Task<Guid?> AddVehicleRent(RentInformation rentInformation);
        Task<Guid?> UpdateVehicleRent(RentInformation rentInformation);
    }
}