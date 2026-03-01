using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;

public record AddVehicleHandlerRequest(DateTime? RegistrationDate, string? FrameId, string? LicensePlate)
    : IRequest<IActionResult>;