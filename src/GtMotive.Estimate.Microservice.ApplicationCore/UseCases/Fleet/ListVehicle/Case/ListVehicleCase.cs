using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Services;


namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Case
{
    /// <summary>
    /// A Handler for a vehicle creation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ListVehicleCase"/> class.
    /// </remarks>
    /// <param name="vehicleService"> the writing service.</param>
    /// <param name="outputPortStandard"> an output standard port.</param>
    public class ListVehicleCase(
        IVehicleService vehicleService,
        IOutputPortStandard<ListVehicleResponse> outputPortStandard) : IUseCase<ListVehicleRequest>
    {
        /// <summary>
        ///  finds the vehicle list for the current page
        /// </summary>
        /// <param name="request">the page configuration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Execute(ListVehicleRequest request)
        {
            if (request == null || request.PageIndex < 0 || request.PageSize > 0)
            {
                request = new ListVehicleRequest(0, 50);
            }

            ArgumentNullException.ThrowIfNull(request);
            var result = await vehicleService.GetVehicles(request.PageIndex, request.PageSize);
            outputPortStandard.StandardHandle(new ListVehicleResponse(result));
        }
    }
}
