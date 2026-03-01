using GtMotive.Estimate.Microservice.ApplicationCore.Common;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.RentVehicle.Commands;

public record RentVehicleResponse(Guid Id) : ResultResponse(Id), IUseCaseOutput;