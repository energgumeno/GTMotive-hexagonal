using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Case;

/// <summary>
///     A Handler for a vehicle creation.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ListVehicleCase" /> class.
/// </remarks>
/// <param name="vehiclePort"> the writing service.</param>
/// <param name="logger">The app logger.</param>
/// <param name="outputPortStandard"> an output standard port.</param>
/// <param name="errorOutputPort"> the error output port.</param>
public class ListVehicleCase(
    IVehiclePort vehiclePort,
    IAppLogger<ListVehicleCase> logger,
    IOutputPortStandard<ListVehicleResponse> outputPortStandard,
    IErrorOutputPort errorOutputPort) : IUseCase<ListVehicleCommand>
{
    /// <summary>
    ///     finds the vehicle list for the current page
    /// </summary>
    /// <param name="request">the page configuration.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task Execute(ListVehicleCommand request)
    {
        try
        {
            if (request.PageIndex < 0 || request.PageSize < 1)
                request = new ListVehicleCommand(Math.Max(0, request.PageIndex),
                    request.PageSize < 1 ? 50 : request.PageSize);

            var (result, totalPages) = await vehiclePort.GetVehicles(_ => true, request.PageIndex, request.PageSize);


            if (result.Count == 0)
                errorOutputPort.NotFoundHandle("vehicle not found");
            else
                outputPortStandard.StandardHandle(new ListVehicleResponse(result, totalPages, request.PageIndex,
                    request.PageSize));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while executing ListVehicleCase.");
            errorOutputPort.GeneralErrorHandle("An unexpected error occurred. Please try again later.");
        }
    }
}