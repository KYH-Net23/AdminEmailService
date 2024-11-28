
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
                    Type type = CheckModelType(body);
                    var email = JsonConvert.DeserializeObject(body, type);
                    var service = CheckEmailType(email);
                    await service!.SendEmailAsync(email);
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

        private IEmailService<T>? CheckEmailType<T>(T email)
        {
            return email switch
            {
                OrderConfirmationModel => (IEmailService<T>?)_orderEmailService,
                VerificationEmailModel => (IEmailService<T>?)_verificationEmailService,
                ResetPasswordModel => (IEmailService<T>?)_resetPasswordService,
                WelcomeEmailModel => (IEmailService<T>?)_welcomeEmailService,
                _ => null
            };
        }

        private Type CheckModelType(string body)
        {
            if (body.Contains("Order")) return typeof(OrderConfirmationModel);
            if (body.Contains("Verification")) return typeof(VerificationEmailModel);
            if (body.Contains("Reset")) return typeof(ResetPasswordModel);
            if (body.Contains("Welcome")) return typeof(WelcomeEmailModel);
            throw new InvalidOperationException("Unknown email type.");
        }

    }
}
