using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Common.NotFound;

internal class OutputPortNotFound(IWebApiPresenter webApiSetter) : IOutputPortNotFound
{
    public void NotFoundHandle(string message)
    {
        webApiSetter.ActionResult = new NotFoundObjectResult(message);
    }
}