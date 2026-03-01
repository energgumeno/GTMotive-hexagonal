
using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.ListVehicle.Commands
{
    public record ListVehicleRequest(int PageIndex, int PageSize) : PagedRequest(PageIndex, PageSize), IUseCaseInput;
}
