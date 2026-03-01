using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands
{
    public record ListVehicleResponse(List<Vehicle> VehiclesList) : IUseCaseOutput;
}
