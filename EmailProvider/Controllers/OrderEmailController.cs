using EmailProvider.EmailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "OrderProvider")]
public class OrderEmailController : ControllerBase
{

    private readonly string _connectionString;
    private readonly OrderEmailService _orderEmailService;

    public OrderEmailController(OrderEmailService orderEmailService, string connectionString)
    {
        _orderEmailService = orderEmailService;
        _connectionString = connectionString;
    }
    [HttpGet]
    public IActionResult SendOrderConfirmation(OrderConfirmationModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _orderEmailService.SendOrderConfirmation(model, _connectionString);
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