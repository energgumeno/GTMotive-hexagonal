using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ListVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FleetController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AddVehicleResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleHandlerRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListVehicleResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetVehicles([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {
        return await mediator.Send(new ListVehicleRequest(pageIndex, pageSize));
    }
}