using EmailProvider.EmailServices;
using EmailProvider.Models.OrderConfirmationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(Policy = "PaymentProvider")]
public class OrderConfirmationController : ControllerBase
{

    private readonly OrderEmailService _orderEmailService;

    public OrderConfirmationController(OrderEmailService orderEmailService)
    {
        _orderEmailService = orderEmailService;
    }
    [HttpPost]
    public IActionResult SendOrderConfirmation([FromBody] OrderConfirmationModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _orderEmailService.SendOrderConfirmation(model);
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