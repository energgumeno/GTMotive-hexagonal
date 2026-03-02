using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Commands;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects.Aggregates;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Case;

public class ProcessRentCreatedCase(IRentVehiclePort rentVehiclePort) : IUseCase<ProcessRentCreatedCommand>
{
    public async Task Execute(ProcessRentCreatedCommand request)
    {
        var newRent = request.Event.RentInformation;

        // Obtener todas las rentas para el mismo vehículo
        var allRents =
            await rentVehiclePort
                .GetVehiclesRentByVehicleId(newRent
                    .VehicleId); // Simplificación, idealmente un método específico en el puerto

        var conflictingRents = allRents
            .Where(r =>
                r.VehicleId == newRent.VehicleId &&
                r.Id != newRent.Id &&
                r.Status == RentStatus.Accepted &&
                ((newRent.TimeRentStart >= r.TimeRentStart && newRent.TimeRentStart <= r.TimeRentEnd) ||
                 (newRent.TimeRentEnd >= r.TimeRentStart && newRent.TimeRentEnd <= r.TimeRentEnd) ||
                 (newRent.TimeRentStart <= r.TimeRentStart && newRent.TimeRentEnd >= r.TimeRentEnd)))
            .ToList();

        if (conflictingRents.Any())
            VehicleRentAggregate.CancelRent(newRent);
        else
            VehicleRentAggregate.AcceptRent(newRent);

        await rentVehiclePort.UpdateVehicleRent(newRent);
        await rentVehiclePort.Save();
    }
}