using Azure.Communication.Email;
using Azure;
using EmailProvider.Models.DataModels;

namespace EmailProvider.EmailServices
{
    public class IdentityEmailService
    {
        public void SendConfirmationMessageAsync(IdentityEmailModel model, string connectionString)
        {
            var emailClient = new EmailClient(connectionString);         

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@beb6ca96-5cd4-43a4-a4bc-6723d17f26d9.azurecomm.net",
                recipientAddress: model.Receiver,
                content: new EmailContent("Verify Your Email-Address.")
                {
                    Html = $"""
                            <html>
                                <body style="font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333333; margin: 0; padding: 20px;">
                                    <div style="max-width: 600px; margin: 0 auto; background: #ffffff; border: 1px solid #dddddd; border-radius: 8px; padding: 20px; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);">
                                        <div style="text-align: center; border-bottom: 1px solid #dddddd; padding-bottom: 10px;">
                                            <h1 style="color: #0073e6; font-size: 24px; margin: 0;">Confirmation Email</h1>
                                        </div>
                                        <div style="margin-top: 20px; line-height: 1.6;">
                                            <p>Hello,</p>
                                            <p>Thank you for your request. Please use the confirmation code below to verify your email:</p>
                                            <div style="display: flex; justify-content: center; gap: 10px; margin: 20px 0;">
                                                {string.Join("", model.PassCode.Select(c => $"""
                                                <div style="width: 40px; height: 50px; border: 1px solid #dddddd; background-color: #f9f9f9; display: flex; justify-content: center; align-items: center; gap: 10px; font-size: 20px; font-weight: bold; color: #333333; border-radius: 5px; box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);">
                                                    {c.ToString()}
                                                </div>
                                                """))}
                                            </div>
                                            <p style="margin-top: 20px;">If you did not request this, please ignore this email.</p>
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

            EmailSendOperation emailSendOperation = emailClient.Send(
                WaitUntil.Completed,
                emailMessage
            );
        }


        public void ResetPasswordAsync(IdentityEmailModel model, string connectionString)
        {
            var emailClient = new EmailClient(connectionString);

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@beb6ca96-5cd4-43a4-a4bc-6723d17f26d9.azurecomm.net",
                recipientAddress: model.Receiver,
                content: new EmailContent("Reset Your Password.")
                {
                    Html = $"""
                            <html>
                                <body style="font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333333; margin: 0; padding: 20px;">
                                    <div style="max-width: 600px; margin: 0 auto; background: #ffffff; border: 1px solid #dddddd; border-radius: 8px; padding: 20px; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);">
                                        <div style="text-align: center; border-bottom: 1px solid #dddddd; padding-bottom: 10px;">
                                            <h1 style="color: #0073e6; font-size: 24px; margin: 0;">Reset Password</h1>
                                        </div>
                                        <div style="margin-top: 20px; line-height: 1.6;">
                                            <p>Hello,</p>
                                            <p>Thank you for your request. Please click the button below to reset your password:</p>
                                            <a href="{model!.PassCode}" style="display: inline-block; margin-top: 20px; padding: 10px 20px; background-color: #0073e6; color: white; text-decoration: none; border-radius: 5px; font-size: 16px;">Reset Password</a>
                                            <p style="margin-top: 20px;">If you did not request this, please ignore this email.</p>
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

            EmailSendOperation emailSendOperation = emailClient.Send(
                WaitUntil.Completed,
                emailMessage
            );
        }

        public void SendWelcomeEmailAsync(IdentityEmailModel model, string connectionString)
        {
            var emailClient = new EmailClient(connectionString);
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

            EmailSendOperation emailSendOperation = emailClient.Send(
                WaitUntil.Completed,
                emailMessage
            );
        }
    }
}
