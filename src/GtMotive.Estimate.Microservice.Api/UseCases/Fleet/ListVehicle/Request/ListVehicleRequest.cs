using GtMotive.Estimate.Microservice.ApplicationCore.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ListVehicle.Request
{
    public record ListVehicleRequest(int PageIndex, int PageSize) : IRequest<IActionResult>;
}
