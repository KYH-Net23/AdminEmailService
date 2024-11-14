using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "PaymentPolicy")]
public class PaymentEmailController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTestEndpoint()
    {
        return Ok();
    }
}