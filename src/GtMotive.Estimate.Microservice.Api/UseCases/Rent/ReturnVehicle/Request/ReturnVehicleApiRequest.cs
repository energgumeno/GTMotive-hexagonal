using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rent.ReturnVehicle.Request;

public record ReturnVehicleApiRequest(Guid? RentId) : IRequest<IActionResult>;