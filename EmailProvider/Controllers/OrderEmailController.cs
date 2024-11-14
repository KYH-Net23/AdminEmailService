using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "OrderPolicy")]
public class OrderEmailController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTestEndpoint()
    {
        return Ok();
    }
}