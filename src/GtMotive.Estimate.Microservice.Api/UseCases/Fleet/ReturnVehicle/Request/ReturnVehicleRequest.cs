using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ReturnVehicle.Request
{
    public record ReturnVehicleRequest(Guid Id) :  IRequest<IActionResult>;

}
