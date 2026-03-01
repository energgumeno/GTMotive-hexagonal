using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Port;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Case;

/// <summary>
///     A Handler for a vehicle creation.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ListVehicleCase" /> class.
/// </remarks>
/// <param name="vehiclePort"> the writing service.</param>
/// <param name="outputPortStandard"> an output standard port.</param>
/// ///
/// <param name="outputPortNotFound"> an output standard port.</param>
public class ListVehicleCase(
    IVehiclePort vehiclePort,
    IOutputPortStandard<ListVehicleResponse> outputPortStandard,
    IOutputPortNotFound outputPortNotFound) : IUseCase<ListVehicleCommand>
{
    /// <summary>
    ///     finds the vehicle list for the current page
    /// </summary>
    /// <param name="request">the page configuration.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task Execute(ListVehicleCommand? request)
    {
        if (request == null || request.PageIndex < 0 || request.PageSize < 1) request = new ListVehicleCommand(0, 50);

        ArgumentNullException.ThrowIfNull(request);
        List<Vehicle?> result;
        int TotalPages;
        using (vehiclePort)
        {
            (result, TotalPages) = await vehiclePort.GetVehicles(request.PageIndex, request.PageSize);
        }

        if (result.Count == 0)
            outputPortNotFound.NotFoundHandle("vehicle not found");
        else
            outputPortStandard.StandardHandle(new ListVehicleResponse(result, TotalPages, request.PageIndex,
                request.PageSize));
    }
}