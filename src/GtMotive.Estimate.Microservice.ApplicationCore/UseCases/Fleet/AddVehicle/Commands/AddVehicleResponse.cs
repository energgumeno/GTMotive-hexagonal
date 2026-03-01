using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Fleet.AddVehicle.Commands;

/// <summary>
/// Represents a response for an added vehicle.
/// </summary>
/// <param name="Id">The unique identifier of the added vehicle.</param>
public record AddVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;