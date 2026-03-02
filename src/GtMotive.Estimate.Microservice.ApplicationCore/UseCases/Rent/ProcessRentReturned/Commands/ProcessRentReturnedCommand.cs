using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentReturned;

public record ProcessRentReturnedCommand(RentVehicleReturnedEvent Event) : IUseCaseInput;
