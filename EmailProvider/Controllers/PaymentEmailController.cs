using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "PaymentProvider")]
public class PaymentEmailController : ControllerBase
{
    [HttpGet]
    public IActionResult SendOrderConfirmation()
    {
        return Ok();
    }
}