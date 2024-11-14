using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "IdentityPolicy")]
public class IdentityEmailController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTestEndpoint()
    {
        return Ok();
    }
}