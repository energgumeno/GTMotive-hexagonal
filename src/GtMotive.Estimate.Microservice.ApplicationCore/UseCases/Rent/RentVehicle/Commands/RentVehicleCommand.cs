#nullable enable
using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.RentVehicle.Commands
{
    public record RentVehicleCommand(string? Fullname,string?  Email,DateTime?  TimeRentStart, DateTime? TimeRentEnd,Guid? VehicleId) : IUseCaseInput;
}
