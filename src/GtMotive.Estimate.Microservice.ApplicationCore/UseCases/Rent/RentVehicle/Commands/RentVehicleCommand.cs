namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;

/// <summary>
///     Represents a command to rent a vehicle.
/// </summary>
/// <param name="Fullname">The full name of the person renting the vehicle.</param>
/// <param name="Email">The email address of the person renting the vehicle.</param>
/// <param name="TimeRentStart">The start time of the rent period.</param>
/// <param name="TimeRentEnd">The end time of the rent period.</param>
/// <param name="VehicleId">The unique identifier of the vehicle being rented.</param>
public record RentVehicleCommand(
    string? Fullname,
    string? Email,
    DateTime? TimeRentStart,
    DateTime? TimeRentEnd,
    Guid? VehicleId) : IUseCaseInput;