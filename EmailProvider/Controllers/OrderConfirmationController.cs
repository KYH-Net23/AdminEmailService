using EmailProvider.EmailServices;
using EmailProvider.EmailServices.EmailQueue;
using EmailProvider.Models.OrderConfirmationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(Policy = "PaymentProvider")]
public class OrderConfirmationController : ControllerBase
{
    private readonly EmailQueueService _emailQueueService;

    public OrderConfirmationController(EmailQueueService emailQueueService)
    {
        _emailQueueService = emailQueueService;
    }
    [HttpPost]
    public async Task<IActionResult> SendOrderConfirmation([FromBody] OrderConfirmationModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _emailQueueService.EnQueueEmailAsync(model);
                return Ok();
            }
            catch
            {
                return BadRequest(ModelState);
            }
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}