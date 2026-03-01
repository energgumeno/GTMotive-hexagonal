using System;
using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands
{
    public record AddVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;
}
