using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Events;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rent.ProcessRentCreated;

public record ProcessRentCreatedCommand(RentVehicleCreatedEvent Event) : IUseCaseInput;
