using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Common.NotFound;

internal class BaseHandler<T>(IWebApiSetter webApiSetter) :IOutputPortStandard<T> where T : IUseCaseOutput
{
    
    public void StandardHandle(T response)
    {
        webApiSetter.ActionResult = new OkObjectResult(response);
    }
}