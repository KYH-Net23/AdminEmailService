using EmailProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController(EmailService emailService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await emailService.GetEmailsAsync();

        return Ok(result);
    }
}
