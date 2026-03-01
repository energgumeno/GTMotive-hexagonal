using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ReturnVehicle.Commands
{
    public record ReturnVehicleCommand(Guid Id) : IUseCaseInput;
}
