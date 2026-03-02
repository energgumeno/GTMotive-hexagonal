using GtMotive.Estimate.Microservice.Api.UseCases.Rent.RentVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rent.RentVehicle.Handlers;

public class RentVehicleApiHandler(IUseCase<RentVehicleCommand> rentVehicleUseCase, IWebApiPresenter webApiPresenter)
    : IRequestHandler<RentVehicleApiRequest, IActionResult>
{
    public async Task<IActionResult> Handle(RentVehicleApiRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await rentVehicleUseCase.Execute(new RentVehicleCommand(
            request.Fullname,
            request.Email,
            request.TimeRentStart,
            request.TimeRentEnd,
            request.VehicleId));

        return webApiPresenter.ActionResult;
    }
}