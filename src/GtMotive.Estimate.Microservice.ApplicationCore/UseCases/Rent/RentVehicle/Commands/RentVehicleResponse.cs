using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;

/// <summary>
///     Represents a response for a rented vehicle.
/// </summary>
/// <param name="Id">The unique identifier of the rent record.</param>
public record RentVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;