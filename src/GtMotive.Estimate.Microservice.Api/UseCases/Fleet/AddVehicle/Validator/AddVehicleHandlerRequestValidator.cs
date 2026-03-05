using FluentValidation;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Validator;

public class AddVehicleHandlerRequestValidator : AbstractValidator<AddVehicleHandlerRequest>
{
    public AddVehicleHandlerRequestValidator()
    {
        RuleFor(x => x.RegistrationDate)
            .NotEmpty().WithMessage("Registration date is required");

        RuleFor(x => x.FrameId)
            .NotEmpty().WithMessage("Frame id is required");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License Plate id is required");
    }
}