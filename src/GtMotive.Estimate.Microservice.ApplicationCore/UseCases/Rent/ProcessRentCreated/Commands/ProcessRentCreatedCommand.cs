using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated.Commands;

public record ProcessRentCreatedCommand(RentVehicleCreatedEvent Event) : IUseCaseInput;