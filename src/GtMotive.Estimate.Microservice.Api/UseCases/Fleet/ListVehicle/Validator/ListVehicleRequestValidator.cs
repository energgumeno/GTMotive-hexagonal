using FluentValidation;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ListVehicle.Request;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.ListVehicle.Validator;

public class ListVehicleRequestValidator : AbstractValidator<ListVehicleRequest>
{
    public ListVehicleRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1");
    }
}