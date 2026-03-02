using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned.Commands;

public record ProcessRentReturnedCommand(RentVehicleReturnedEvent Event) : IUseCaseInput;