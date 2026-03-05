using FluentValidation;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Common.Validation;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IErrorOutputPort errorOutputPort,
    IWebApiPresenter webApiPresenter) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                var message = string.Join(", ", failures.Select(f => f.ErrorMessage));
                errorOutputPort.BadRequestHandle(message);

                if (typeof(TResponse) == typeof(IActionResult))
                {
                    return (TResponse)webApiPresenter.ActionResult!;
                }

                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}
