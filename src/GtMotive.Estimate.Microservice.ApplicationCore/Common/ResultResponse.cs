using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common;

/// <summary>
/// Represents a result response.
/// </summary>
/// <param name="Id">The unique identifier.</param>
public record ResultResponse(Guid Id) : IUseCaseOutput;