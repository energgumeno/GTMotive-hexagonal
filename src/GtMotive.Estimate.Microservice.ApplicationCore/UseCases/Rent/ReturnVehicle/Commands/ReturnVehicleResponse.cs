using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands
{
    public record ReturnVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;
}
