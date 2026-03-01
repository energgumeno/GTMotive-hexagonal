using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.Common.NotFound;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ListVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.Common;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ListVehicle.Handlers
{
    public class ListVehicleHandler(IUseCase<ListVehicleCommand> listVehiclePort, IWebApiPresenter webApiPresenter)
        : IRequestHandler<ListVehicleRequest, IActionResult>
    {
        public async Task<IActionResult> Handle(ListVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            await listVehiclePort.Execute(new ListVehicleCommand(request.PageIndex, request.PageSize));

            return webApiPresenter.ActionResult;
        }
    }
}