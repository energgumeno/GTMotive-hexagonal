using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common
{
    public record PagedResponse<T>(
        List<T?> Data,
        int TotalCount,
        int PageNumber,
        int PageSize
    )
        where T : class
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
