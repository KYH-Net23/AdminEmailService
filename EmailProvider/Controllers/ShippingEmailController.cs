using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "ShippingPolicy")]
public class ShippingEmailController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTestEndpoint()
    {
        return Ok();
    }
}