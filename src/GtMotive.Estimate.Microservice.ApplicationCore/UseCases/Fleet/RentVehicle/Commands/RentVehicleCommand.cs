#nullable enable
using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.RentVehicle.Commands
{
    public record RentVehicleCommand(Guid VehicleId, string? FullName, DateTime? Date) : IUseCaseInput;
}
