using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    public record ResultResponse(Guid Id):IUseCaseOutput;
}
