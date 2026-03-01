namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands
{
    public record ReturnVehicleCommand(Guid? RentId) : IUseCaseInput;
}
