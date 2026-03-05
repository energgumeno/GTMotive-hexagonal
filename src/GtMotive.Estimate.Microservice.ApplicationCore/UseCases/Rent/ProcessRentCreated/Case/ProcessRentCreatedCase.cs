using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Commands;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;
using Microsoft.VisualBasic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Case;

public class ProcessRentCreatedCase(IRentVehiclePort rentVehiclePort) : IUseCase<ProcessRentCreatedCommand>
{
    public async Task Execute(ProcessRentCreatedCommand request)
    {
        var newRent = request.Event.RentInformation;

        // Obtener todas las rentas para el mismo vehículo
        var conflictingRents =
            await rentVehiclePort .GetVehiclesRent(information => information.IsConflict(newRent) );

        VehicleRentAggregate.ConfirmRentState(newRent, conflictingRents);
  
        await rentVehiclePort.UpdateVehicleRent(newRent);
        await rentVehiclePort.Save();
    }
}