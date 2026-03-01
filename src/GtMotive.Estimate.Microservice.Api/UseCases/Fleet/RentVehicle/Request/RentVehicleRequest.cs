using GtMotive.Estimate.Microservice.ApplicationCore.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.RentVehicle.Request
{
    public record RentVehicleRequest(string? Fullname,string?  Email,DateTime?  TimeRentStart, DateTime? TimeRentEnd,Guid? VehicleId) :  IRequest<IActionResult>;
}
