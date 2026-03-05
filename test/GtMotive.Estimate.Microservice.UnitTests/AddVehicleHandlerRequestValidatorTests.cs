using FluentValidation.TestHelper;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Request;
using GtMotive.Estimate.Microservice.Api.UseCases.Fleet.AddVehicle.Validator;

namespace GtMotive.Estimate.Microservice.UnitTests;

public class AddVehicleHandlerRequestValidatorTests
{
    private readonly AddVehicleHandlerRequestValidator _validator;

    public AddVehicleHandlerRequestValidatorTests()
    {
        _validator = new AddVehicleHandlerRequestValidator();
    }

    [Fact]
    public void Validator_ShouldHaveError_WhenPropertiesAreEmpty()
    {
        var request = new AddVehicleHandlerRequest(null, string.Empty, " ");
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.RegistrationDate);
        result.ShouldHaveValidationErrorFor(x => x.FrameId);
        result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
    }

    [Fact]
    public void Validator_ShouldNotHaveError_WhenPropertiesAreFilled()
    {
        var request = new AddVehicleHandlerRequest(DateTime.Now, "FRAME123", "ABC-123");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}