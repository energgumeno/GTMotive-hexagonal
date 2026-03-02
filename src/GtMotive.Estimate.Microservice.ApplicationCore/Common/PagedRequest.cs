namespace GtMotive.Estimate.Microservice.ApplicationCore.Common;

/// <summary>
///     Represents a paged request.
/// </summary>
/// <param name="PageIndex">The index of the page.</param>
/// <param name="PageSize">The size of the page.</param>
public record PagedRequest(int PageIndex, int PageSize);