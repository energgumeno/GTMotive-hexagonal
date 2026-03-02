using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ReturnVehicle.Commands;

/// <summary>
///     Represents a response for a returned vehicle.
/// </summary>
/// <param name="Id">The unique identifier of the rent record.</param>
public record ReturnVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;