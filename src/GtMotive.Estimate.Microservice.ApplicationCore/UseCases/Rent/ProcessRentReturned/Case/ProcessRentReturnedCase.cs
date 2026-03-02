using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned;

public class ProcessRentReturnedCase(IRentVehiclePort rentVehiclePort) : IUseCase<ProcessRentReturnedCommand>
{
    public async Task Execute(ProcessRentReturnedCommand request)
    {
        var rent = request.Event.RentInformation;
        
        VehicleRentAggregate.ReturnVehicle(rent);

        await rentVehiclePort.UpdateVehicleRent(rent);
        await rentVehiclePort.Save();
    }
}
