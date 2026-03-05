using FluentValidation;
using GtMotive.Estimate.Microservice.Api.UseCases.Rent.ReturnVehicle.Request;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rent.ReturnVehicle.Validator;

public class ReturnVehicleApiRequestValidator : AbstractValidator<ReturnVehicleApiRequest>
{
    public ReturnVehicleApiRequestValidator()
    {
        RuleFor(x => x.RentId)
            .NotEmpty().WithMessage("Rent ID is required");
    }
}
