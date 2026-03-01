using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.Common;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands
{
    public record ListVehicleResponse(List<Vehicle?> Data,int TotalCount, int PageNumber, int PageSize) : PagedResponse<Vehicle>(Data, TotalCount, PageNumber, PageSize), IUseCaseOutput
    {
        
    }
}
