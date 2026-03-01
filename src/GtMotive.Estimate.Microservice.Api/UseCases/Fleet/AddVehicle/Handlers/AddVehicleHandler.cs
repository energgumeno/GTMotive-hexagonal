using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.Common.NotFound;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Handlers
{
    /// <summary>
    /// A Handler for a vehicle creation.
    /// </summary>
    public class AddVehicleHandler(IUseCase<AddVehicleCommand> addVehiclePort, IWebApiPresenter webApiPresenter)
        : IRequestHandler<AddVehicleHandlerRequest, IActionResult>
    {
        public async Task<IActionResult> Handle(AddVehicleHandlerRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await addVehiclePort.Execute(new AddVehicleCommand(
                request.RegistrationDate,
                request.FrameId,
                request.LicensePlate));

            return webApiPresenter.ActionResult;
        }
    }
}