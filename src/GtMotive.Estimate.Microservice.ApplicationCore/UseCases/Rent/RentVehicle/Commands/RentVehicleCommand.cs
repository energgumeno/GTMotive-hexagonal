#nullable enable
namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands
{
    public record RentVehicleCommand(string? Fullname,string?  Email,DateTime?  TimeRentStart, DateTime? TimeRentEnd,Guid? VehicleId) : IUseCaseInput;
}
