using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.RentVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.RentVehicle.Handlers
{
    public class RentVehicleHandler(IUseCase<RentVehicleCommand> rentVehiclePort, IWebApiPresenter webApiPresenter)
        : IRequestHandler<RentVehicleRequest, IActionResult>
    {
        public async Task<IActionResult> Handle(RentVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            await rentVehiclePort.Execute(new RentVehicleCommand(request.Fullname, request.Email, request.TimeRentStart,
                request.TimeRentEnd, request.VehicleId));

            return webApiPresenter.ActionResult;
        }

    }
}