
using Azure.Messaging.ServiceBus;
using EmailProvider.Interfaces;
using EmailProvider.Models.DataModels;
using EmailProvider.Models.OrderConfirmationModels;
using Newtonsoft.Json;
using System.Text;

namespace EmailProvider.EmailServices.EmailQueue
{
    public class EmailProcessingService : BackgroundService
    {

        private readonly ServiceBusProcessor _processor;
        private readonly OrderEmailService _orderEmailService;
        private readonly WelcomeEmailService _welcomeEmailService;
        private readonly VerificationEmailService _verificationEmailService;
        private readonly ResetPasswordService _resetPasswordService;

        public EmailProcessingService(ServiceBusClient client, OrderEmailService orderEmailService, string config, WelcomeEmailService welcomeEmailService, VerificationEmailService verificationEmailService, ResetPasswordService resetPasswordService)
        {
            _processor = client.CreateProcessor(config);
            _orderEmailService = orderEmailService;
            _welcomeEmailService = welcomeEmailService;
            _verificationEmailService = verificationEmailService;
            _resetPasswordService = resetPasswordService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += async args =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(args.Message.Body);
                    var emailType = DetermineEmailType(body);
                    var email = DeserializeEmail(body, emailType);
                    await SendEmailByType(email, emailType);
                }
                catch
                {
                    await args.AbandonMessageAsync(args.Message);
                }

            };

            _processor.ProcessErrorAsync += _ => Task.CompletedTask;

            await _processor.StartProcessingAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            await _processor.StopProcessingAsync(stoppingToken);
        }

        private string DetermineEmailType(string body)
        {
            return body switch
            {
                string b when b.Contains("\"EmailType\":\"Order\"") => "Order",
                string b when b.Contains("\"EmailType\":\"Welcome\"") => "Welcome",
                string b when b.Contains("\"EmailType\":\"Verification\"") => "Verification",
                string b when b.Contains("\"EmailType\":\"Reset\"") => "Reset",
                _ => throw new InvalidOperationException("Unknown email type")
            };
        }

        private object DeserializeEmail(string body, string emailType)
        {
            return emailType switch
            {
                "Order" => JsonConvert.DeserializeObject<OrderConfirmationModel>(body)!,
                "Welcome" => JsonConvert.DeserializeObject<WelcomeEmailModel>(body)!,
                "Verification" => JsonConvert.DeserializeObject<VerificationEmailModel>(body)!,
                "Reset" => JsonConvert.DeserializeObject<ResetPasswordModel>(body)!,
                _ => throw new InvalidOperationException("Unknown email type")
            };
        }

        private async Task SendEmailByType(object email, string emailType)
        {
            switch (emailType)
            {
                case "Order":
                    await _orderEmailService.SendEmailAsync((OrderConfirmationModel)email);
                    break;
                case "Welcome":
                    await _welcomeEmailService.SendEmailAsync((WelcomeEmailModel)email);
                    break;
                case "Verification":
                    await _verificationEmailService.SendEmailAsync((VerificationEmailModel)email);
                    break;
                case "Reset":
                    await _resetPasswordService.SendEmailAsync((ResetPasswordModel)email);
                    break;
                default:
                    throw new InvalidOperationException("Unknown email type");
            }
        }

    }
}
