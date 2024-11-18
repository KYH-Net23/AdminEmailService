using Azure.Communication.Email;
using System.Text;
using EmailProvider.Models.OrderConfirmationModels;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Azure;

namespace EmailProvider.EmailServices
{
    public class OrderEmailService
    {

        private readonly string _connectionString;
        private readonly EmailClient _client;

        public OrderEmailService(string connectionString)
        {
            _connectionString = connectionString;
            _client = new EmailClient(_connectionString);
        }

        public void SendOrderConfirmation(OrderConfirmationModel model)
        {
            StringBuilder builder = new StringBuilder();
            foreach(var product in model.Products)
            {
                builder.AppendLine($"<td style=\"border: 1px solid #dddddd; max-width: 150px; object-fit: cover; padding: 10px; text-align: left; background-color: #f2f2f2;\"><img src=\"{product?.ImageUrl}\" /></td>");
                builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product!.Name}</td>");
                builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product.Amount}</td>");
                builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product.Price:C2}</td>");
                builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product?.DiscountedPrice}</td>");
            }
           
            var tracking = "";

            if (model?.Shipping.TrackingLink != null)
            {
                tracking = "<p><strong>Tracking:</strong> <a href=\\\"{model.Shipping.TrackingLink}\\\" style=\\\"color: #007BFF;\\\">Track your package</a></p>";
            }

            var emailBody = $$"""
            <!DOCTYPE html>
            <html>
            <head>
            </head>
            <body style="font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f9f9f9;">
                <div style="max-width: 900px; margin: 20px auto; background: #ffffff; border: 1px solid #dddddd; padding: 20px; text-align: center;">
                    <h1 style="font-size: 24px; font-weight: bold; color: #333333; margin-bottom: 20px;">Order Confirmation</h1>
                    <p style="font-size: 16px; color: #555555; margin-bottom: 30px;">Thank you for your order!</p>
                    <div style="text-align: left; font-size: 14px; color: #333333; line-height: 1.5;">
                        <h2 style="font-size: 18px; color: #333333; margin-bottom: 10px;">Shipping Information</h2>
                        <p><strong>Name:</strong> {{model.Shipping.FullName}}</p>
                        <p><strong>Delivery Address:</strong> {{model.Shipping.CustomerDeliveryAddress}}</p>
                        <p><strong>Postal Pickup Address:</strong> {{model.Shipping.PostalPickUpAddress}}</p>
                        <p><strong>Phone:</strong> {{model.Shipping.PhoneNumber}}</p>
                        <p><strong>Expected Arrival:</strong> {{model.Shipping.OrderArrival}}</p>
                       {{tracking}}
                    </div>
                    <div style="text-align: left; font-size: 14px; color: #333333; line-height: 1.5;">
                        <h2 style="font-size: 18px; color: #333333; margin-bottom: 10px;">Invoice Information</h2>
                        <p><strong>Name:</strong> {{model.Invoice.FullName}}</p>
                        <p><strong>Address:</strong> {{model.Invoice.StreetAddress}}, {{model.Invoice.City}}, {{model.Invoice.PostalCode}}, {{model.Invoice.Country}}</p>
                        <p><strong>Payment Method:</strong> {{model.Invoice.PaymentOption}}</p>
                    </div>
                    <div style="text-align: left; font-size: 14px; color: #333333; line-height: 1.5;">
                        <h2 style="font-size: 18px; color: #333333; margin-bottom: 10px;">Order Summary</h2>
                        <table style="width: 100%; border-collapse: collapse; margin: 20px 0;">
                            <thead>
                                <tr>
                                    <th style="border: 1px solid #dddddd; max-width: 150px; padding: 10px; text-align: left; background-color: #f2f2f2;"></th>
                                    <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Product</th>
                                    <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Amount</th>
                                    <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Price</th>
                                    <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Discounted Price</th>
                                </tr>
                            </thead>
                            <tbody>
                             {{builder}}
                            </tbody>
                        </table>
                        <p style = "text-align: right; font-size: 16px; font-weight: bold;" > Order Total: {{ model.OrderTotal:C}}</p>
                    </div>
                    <p style = "margin-top: 30px; font-size: 12px; color: #777777;" > Rika – It will arrive, eventually! </p>
                </div>
            </body>
            </html>
            """;

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@beb6ca96-5cd4-43a4-a4bc-6723d17f26d9.azurecomm.net",
                recipientAddress: model.ReceivingEmail,
                content: new EmailContent("Order Confirmation")
                {
                    Html = emailBody
                }
            );

            var sendOperation = _client.Send(WaitUntil.Completed, emailMessage);
        }
    }
}
