using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ReturnVehicle.Commands
{
    public record ReturnVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;
}
