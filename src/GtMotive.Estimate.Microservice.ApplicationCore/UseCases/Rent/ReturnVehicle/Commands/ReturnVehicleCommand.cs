namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands;

/// <summary>
///     Represents a command to return a vehicle.
/// </summary>
/// <param name="RentId">The unique identifier of the rent record.</param>
public record ReturnVehicleCommand(Guid? RentId) : IUseCaseInput;