using System;
using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.RentVehicle.Commands
{
    public record RentVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;
}
