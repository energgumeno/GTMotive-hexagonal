using GtMotive.Estimate.Microservice.Api.UseCases.Rent.RentVehicle.Request;
using GtMotive.Estimate.Microservice.Api.UseCases.Rent.ReturnVehicle.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RentVehicle([FromBody] RentVehicleApiRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost("return")]
    [ProducesResponseType(typeof(IActionResult), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ReturnVehicle([FromBody] ReturnVehicleApiRequest request)
    {
        return await mediator.Send(request);
    }
}