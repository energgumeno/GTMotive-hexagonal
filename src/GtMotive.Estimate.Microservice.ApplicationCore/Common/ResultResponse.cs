using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common;

public record ResultResponse(Guid Id) : IUseCaseOutput;