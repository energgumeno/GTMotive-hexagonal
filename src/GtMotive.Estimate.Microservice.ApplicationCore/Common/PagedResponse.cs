namespace GtMotive.Estimate.Microservice.ApplicationCore.Common;

/// <summary>
///     Represents a paged response.
/// </summary>
/// <typeparam name="T">The type of the data.</typeparam>
/// <param name="Data">The list of data.</param>
/// <param name="TotalCount">The total count of items.</param>
/// <param name="PageNumber">The number of the page.</param>
/// <param name="PageSize">The size of the page.</param>
public record PagedResponse<T>(
    List<T> Data,
    long TotalCount,
    int PageNumber,
    int PageSize
)
    where T : class
{
    /// <summary>
    ///     Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}