using Azure.Communication.Email;
using Azure;
using EmailProvider.Models.DataModels;
using EmailProvider.Interfaces;

namespace EmailProvider.EmailServices
{
    public class WelcomeEmailService : IEmailService<IdentityEmailModel>
    {

        private readonly string _connectionString;

        public WelcomeEmailService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SendEmailAsync(IdentityEmailModel model)
        {
            var emailClient = new EmailClient(_connectionString);
            var customerName = model.ExtraContent as string;

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@beb6ca96-5cd4-43a4-a4bc-6723d17f26d9.azurecomm.net",
                recipientAddress: model.Receiver,
                content: new EmailContent($"Welcome {customerName}")
                {
                    Html = $"""
                            <html>
                                <body style="font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333333; margin: 0; padding: 20px;">
                                    <div style="max-width: 600px; margin: 0 auto; background: #ffffff; border: 1px solid #dddddd; border-radius: 8px; padding: 20px; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);">
                                        <div style="text-align: center; border-bottom: 1px solid #dddddd; padding-bottom: 10px;">
                                            <h1 style="color: #0073e6; font-size: 24px; margin: 0;">Welcome to Rika!</h1>
                                        </div>
                                        <div style="margin-top: 20px; line-height: 1.6;">
                                            <p>Dear {model.Receiver},</p>
                                            <p>Welcome to Rika Solutions! We are excited to have you on board. Explore our services and take the first step towards your goals.</p>
                                            <a href="{model!.PassCode}" style="display: inline-block; margin-top: 20px; padding: 10px 20px; background-color: #0073e6; color: white; text-decoration: none; border-radius: 5px; font-size: 16px;">Get Started</a>
                                            <p style="margin-top: 20px;">If you have any questions, feel free to reach out to our support team. We're here to help!</p>
                                        </div>
                                        <div style="text-align: center; margin-top: 20px; font-size: 12px; color: #777777;">
                                            <p>&copy; 2024 Rika Solutions. All rights reserved.</p>
                                        </div>
                                    </div>
                                </body>
                            </html>
                        """
                }

            );

            await emailClient.SendAsync(
                WaitUntil.Completed,
                emailMessage
            );
        }
    }
}
