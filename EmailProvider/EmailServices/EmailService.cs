using Azure;
using Azure.Communication.Email;
using EmailProvider.Models.DataModels;
using System.Text;

namespace EmailProvider.EmailServices
{
    public class EmailService
    {
        public void SendDemoEmail(string connectionString)
        {
            var emailClient = new EmailClient(connectionString);


            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@beb6ca96-5cd4-43a4-a4bc-6723d17f26d9.azurecomm.net",
                content: new EmailContent("Restore Password")
                {
                    PlainText = "Hello World via e-post.",
                    Html = @"
		<html>
			<body>
				<h1>Hello World via e-post.</h1>
			</body>
		</html>"
                },
            recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress("developer.ivering@outlook.com") }));


            EmailSendOperation emailSendOperation = emailClient.Send(
                WaitUntil.Completed,
                emailMessage);
        }


    }
}
