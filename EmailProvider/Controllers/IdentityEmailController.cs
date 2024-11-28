using EmailProvider.EmailServices;
using EmailProvider.EmailServices.EmailQueue;
using EmailProvider.Models.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(Policy = "IdentityProvider")]
public class IdentityEmailController : ControllerBase
{

    private readonly string _connectionString;
    private readonly string _accessToken;
    private readonly EmailQueueService _emailQueueService;

    public IdentityEmailController(IConfiguration config, EmailQueueService emailService)
    {
        _connectionString = config["Rika-Email-Connection-String"]!;
        _accessToken = config["Email-Service-Token-AccessKey"]!;
        _emailQueueService = emailService;
    }

    [HttpPost("/confirm")]
    public async Task<IActionResult> ConfirmEmail([FromBody] VerificationEmailModel model)
    {
        if (ModelState.IsValid)
        {
            await _emailQueueService.EnQueueEmailAsync(model);
            return Ok();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPost("/reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            await _emailQueueService.EnQueueEmailAsync(model);
            return Ok();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPost("/welcome")]
    public async Task<IActionResult> WelcomeEmail([FromBody] WelcomeEmailModel model)
    {
        if (ModelState.IsValid)
        {
            await _emailQueueService.EnQueueEmailAsync(model);
            return Ok();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}