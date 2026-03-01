using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands;

/// <summary>
/// Represents a command to list vehicles with pagination.
/// </summary>
/// <param name="PageIndex">The index of the page.</param>
/// <param name="PageSize">The size of the page.</param>
public record ListVehicleCommand(int PageIndex, int PageSize) : PagedRequest(PageIndex, PageSize), IUseCaseInput;