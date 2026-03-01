using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Common.NotFound;

internal class WebApiPresenter : IWebApiPresenter, IWebApiSetter
{
    public IActionResult ActionResult { get; set; } = new NotFoundResult();
}