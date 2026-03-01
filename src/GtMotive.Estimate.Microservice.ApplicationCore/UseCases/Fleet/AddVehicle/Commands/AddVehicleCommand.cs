#nullable enable
using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands
{
    /// <summary>
    /// A command for a vehicle creation.
    /// </summary>
    /// <param name="RegistrationDate"> registration date.</param>
    /// <param name="FrameId">Frame Id.</param>
    /// <param name="LicensePlate">License plate.</param>
    public record AddVehicleCommand(DateTime? RegistrationDate, string? FrameId, string? LicensePlate) : IUseCaseInput;
}
