using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases;

internal interface IWebApiSetter
{
    IActionResult ActionResult { set; }
}