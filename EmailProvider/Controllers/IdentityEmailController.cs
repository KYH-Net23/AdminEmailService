using EmailProvider.EmailServices;
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
    private readonly IdentityEmailService _emailService;

    public IdentityEmailController(IConfiguration config, IdentityEmailService emailService)
    {
        _connectionString = config["Rika-Email-Connection-String"]!;
        _accessToken = config["Email-Service-Token-AccessKey"]!;
        _emailService = emailService;
    }

    [HttpPost("/confirm")]
    public IActionResult ConfirmEmail([FromBody] IdentityEmailModel model)
    {
        if (ModelState.IsValid)
        {
            _emailService.SendConfirmationMessageAsync(model, _connectionString);
            return Ok();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPost("/reset")]
    public IActionResult ResetPassword([FromBody] IdentityEmailModel model)
    {
        if (ModelState.IsValid)
        {
            _emailService.ResetPasswordAsync(model, _connectionString);
            return Ok();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPost("/welcome")]
    public IActionResult WelcomeEmail([FromBody] IdentityEmailModel model)
    {
        if (ModelState.IsValid)
        {
            _emailService.SendWelcomeEmailAsync(model, _connectionString);
            return Ok();
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}