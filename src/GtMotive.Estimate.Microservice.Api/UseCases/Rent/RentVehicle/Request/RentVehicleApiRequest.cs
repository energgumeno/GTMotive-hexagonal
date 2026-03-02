using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rent.RentVehicle.Request;

public record RentVehicleApiRequest(
    string? Fullname,
    string? Email,
    DateTime? TimeRentStart,
    DateTime? TimeRentEnd,
    Guid? VehicleId) : IRequest<IActionResult>;
