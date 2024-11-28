using EmailProvider.EmailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly string _accessToken;

        public EmailController(IConfiguration config)
        {
            _connectionString = config["Rika-Email-Connection-String"]!;
            _accessToken = config["Email-Service-Token-AccessKey"]!;
        }

        [HttpGet]
        public string GetToken()
        {
            var service = new TokenGeneratorService();
            return service.GenerateAccessToken(_accessToken);
        }

    }
}
