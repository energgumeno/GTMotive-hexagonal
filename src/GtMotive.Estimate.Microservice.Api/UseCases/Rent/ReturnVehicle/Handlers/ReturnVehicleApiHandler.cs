using GtMotive.Estimate.Microservice.Api.UseCases.Rent.ReturnVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rent.ReturnVehicle.Handlers;

public class ReturnVehicleApiHandler(
    IUseCase<ReturnVehicleCommand> returnVehicleUseCase,
    IWebApiPresenter webApiPresenter)
    : IRequestHandler<ReturnVehicleApiRequest, IActionResult>
{
    public async Task<IActionResult> Handle(ReturnVehicleApiRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await returnVehicleUseCase.Execute(new ReturnVehicleCommand(request.RentId));

        return webApiPresenter.ActionResult;
    }
}