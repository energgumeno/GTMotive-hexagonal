using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    public record PagedResult<T>(
        IEnumerable<T> Data,
        int TotalCount,
        int PageNumber,
        int PageSize
    )
        where T : class
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
