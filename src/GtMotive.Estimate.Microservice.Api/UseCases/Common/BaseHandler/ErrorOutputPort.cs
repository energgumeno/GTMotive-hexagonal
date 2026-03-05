using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Common.BaseHandler;

internal class ErrorOutputPort(IWebApiPresenter webApiSetter) : IErrorOutputPort
{
    public void NotFoundHandle(string message)
    {
        webApiSetter.ActionResult = new NotFoundObjectResult(message);
    }

    public void BadRequestHandle(string message)
    {
        webApiSetter.ActionResult = new BadRequestObjectResult(message);
    }

    public void GeneralErrorHandle(string message)
    {
        webApiSetter.ActionResult = new ObjectResult(message) { StatusCode = 500 };
    }
}
