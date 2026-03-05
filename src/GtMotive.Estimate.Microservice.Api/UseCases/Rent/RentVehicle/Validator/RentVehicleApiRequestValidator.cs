using FluentValidation;
using GtMotive.Estimate.Microservice.Api.UseCases.Rent.RentVehicle.Request;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rent.RentVehicle.Validator;

public class RentVehicleApiRequestValidator : AbstractValidator<RentVehicleApiRequest>
{
    public RentVehicleApiRequestValidator()
    {
        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("Fullname is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email is required");

        RuleFor(x => x.TimeRentStart)
            .NotEmpty().WithMessage("Time rent start is required");

        RuleFor(x => x.TimeRentEnd)
            .NotEmpty().WithMessage("Time rent end is required");

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle ID is required");
    }
}