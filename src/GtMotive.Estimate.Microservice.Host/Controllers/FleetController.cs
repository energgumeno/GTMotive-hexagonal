using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ListVehicle.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FleetController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(IActionResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleHandlerRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IActionResult), 200)]
    public async Task<IActionResult> GetVehicles([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {
        return await mediator.Send(new ListVehicleRequest(pageIndex, pageSize));
    }
}
