using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Commands;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;
using Microsoft.VisualBasic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Case;

public class ProcessRentCreatedCase(IRentVehiclePort rentVehiclePort, IAppLogger<ProcessRentCreatedCase> logger)
    : IUseCase<ProcessRentCreatedCommand>
{
    public async Task Execute(ProcessRentCreatedCommand request)
    {
        try
        {
            var newRent = request.Event.RentInformation;

            var conflictingRents =
                await rentVehiclePort.GetVehiclesRent(information => information.IsConflict(newRent));

            VehicleRentAggregate.ConfirmRentState(newRent, conflictingRents);

            await rentVehiclePort.UpdateVehicleRent(newRent);
            await rentVehiclePort.Save();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}