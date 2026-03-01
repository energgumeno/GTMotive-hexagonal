using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ReturnVehicle.Request;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ReturnVehicle.Handlers
{
    public class ReturnVehicleHandler(IUseCase<ReturnVehicleCommand> rentVehiclePort, IWebApiPresenter webApiPresenter) : IRequestHandler<ReturnVehicleRequest, IActionResult>
    {
        public async  Task<IActionResult> Handle(ReturnVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            await rentVehiclePort.Execute(new ReturnVehicleCommand(request.Id));
            return webApiPresenter.ActionResult;
            
        }

        
    }
}
