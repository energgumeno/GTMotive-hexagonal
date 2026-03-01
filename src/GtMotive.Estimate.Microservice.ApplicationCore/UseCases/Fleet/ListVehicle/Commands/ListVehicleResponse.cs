using GtMotive.Estimate.Microservice.ApplicationCore.Common;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;

/// <summary>
/// Represents a response with a list of vehicles and pagination information.
/// </summary>
/// <param name="Data">The list of vehicles.</param>
/// <param name="TotalCount">The total count of items.</param>
/// <param name="PageNumber">The number of the page.</param>
/// <param name="PageSize">The size of the page.</param>
public record ListVehicleResponse(List<Vehicle?> Data, int TotalCount, int PageNumber, int PageSize)
    : PagedResponse<Vehicle>(Data, TotalCount, PageNumber, PageSize), IUseCaseOutput
{
}