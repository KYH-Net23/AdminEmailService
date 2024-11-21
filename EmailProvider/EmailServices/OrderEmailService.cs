using Azure.Communication.Email;
using System.Text;
using EmailProvider.Models.OrderConfirmationModels;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Azure;
using System.Reflection;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;

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
            //StringBuilder builder = new StringBuilder();
            //foreach (var product in model.Products)
            //{
            //    builder.AppendLine($"<td style=\"border: 1px solid #dddddd; max-width: 150px; object-fit: cover; padding: 10px; text-align: left; background-color: #f2f2f2;\"><img src=\"{product?.ImageUrl}\" /></td>");
            //    builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product!.Name}</td>");
            //    builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product.Amount}</td>");
            //    builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product.Price:C2}</td>");
            //    builder.AppendLine($"<td style=\"border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;\">{product?.DiscountedPrice}</td>");
            //}




            StringBuilder builder = new StringBuilder();
            foreach (var product in model.Products)
            {
                builder.AppendLine($"<tr>");
                builder.AppendLine($"<td style = \"padding: 24px;\">");
                builder.AppendLine($"<table width = \"100%\" cellpadding = \"0\" cellspacing = \"0\" border = \"0\" style = \"font-family: Tahoma, Arial, sans-serif;\">");
                builder.AppendLine($"<tr>");
                builder.AppendLine($"<td style = \"padding-right: 16px; vertical-align: top; width: 150px;\">");
                builder.AppendLine($"<img src = \"{product.ImageUrl}\" alt =\"product image\" style = \"max-height: 150px; width: 100%; object-fit: cover;\">");
                builder.AppendLine($"</td>");
                builder.AppendLine($"<td style = \"padding-right: 16px; vertical-align: top;\">");
                builder.AppendLine($"<h3 style = \"margin: 0 0 8px; border-bottom: 1px solid #e8e8e8; font-size: 18px; font-weight: bold;\" > {product.Name} </h3>");
                builder.AppendLine($"<p style = \"margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;\">{product.Description}</p >");
                builder.AppendLine($"</td>");
                builder.AppendLine($"<td style = \"vertical-align: bottom; text-align: right;\">");
                builder.AppendLine($"<p style = \"margin: 0; color: #000; font-size: 14px;\" > Quantity: {product.Amount} pcs </p>");
                if (product.DiscountedPrice == null)
                {
                    builder.AppendLine($"<p style = \"margin: 0; color: #000; font-size: 14px;\" > Price: ${product.Price} </p>");                 
                }
                else
                {
                    builder.AppendLine($"<s style = \"margin: 0; color: #000; font-size: 14px;\" > Price: ${product.Price} </s>");
                    builder.AppendLine($"<p style = \"margin: 4px 0 0; color: #000; font-size: 14px;\" > Discounted Price: ${product.DiscountedPrice} </p>");
                }
                builder.AppendLine($"</td>");
                builder.AppendLine($"</tr>");
                builder.AppendLine($"</table>");
                builder.AppendLine($"</td>");
                builder.AppendLine($"</tr>");

            }


            var deliveryAddress = model.Shipping.PostalAgentDeliveryInformation?.StreetAddress ?? model.Shipping.CustomerDeliveryInformation.StreetAddress;
            var phoneNumber = model.Shipping.PostalAgentDeliveryInformation?.PhoneNumber ?? model.Shipping.CustomerDeliveryInformation.PhoneNumber;

            var tracking = "";

            if (model?.Shipping.TrackingLink != null)
            {
                tracking = "<p><strong>Tracking:</strong> <a href=\\\"{model.Shipping.TrackingLink}\\\" style=\\\"color: #007BFF;\\\">Track your package</a></p>";
            }


            var temp = $$"""
            <!DOCTYPE html>
            <html>
            <head>
            </head>
            <body>
            <div style="font-family: 'Tahoma', Arial, sans-serif; margin-bottom: 72px">
                        <h1 style="text-align: center; margin-bottom: 36px; font-family: 'Tahoma', Arial, sans-serif;">Here it comes! Your order has been shipped.</h1>
                        <a style="max-width: 150px; text-decoration: none; width: auto; background: black; color: white; padding: 12px 24px; align-items: center; border-radius: 24px; font-family: 'Tahoma', Arial, sans-serif;" href="{{model?.Shipping.TrackingLink}}">Track package</a>
                        </div>
                        {{builder}}                     
              <div style="text-align: right; padding-top: 12px; font-family: 'Tahoma', Arial, sans-serif;">
                <h3 style="font-size: 18px; font-weight: bold; color: #333; font-family: 'Tahoma', Arial, sans-serif;">Total: ${{model!.OrderTotal}}</h3>
              </div>

              <div style="display: flex; justify-content: center;">
                <a style="max-width: 150px; text-decoration: none; width: auto; background: black; color: white; padding: 12px 24px; text-align: center; border-radius: 24px; font-family: 'Tahoma', Arial, sans-serif;" href="{{model?.Invoice.InvoiceUrl}}">View Receipt</a>
              </div>

              <hr style="border: none; border-top: 1px solid #e5e5e5; margin: 24px 0;">

              <div style="display: flex; justify-content: space-evenly; gap: 16px;">
                <div style="text-align: center; font-family: 'Tahoma', Arial, sans-serif;">
                  <h3 style="font-size: 16px; margin-bottom: 8px; color: #333; font-family: 'Tahoma', Arial, sans-serif;">Shipping From</h3>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">Rika Centrallager</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">12345 Stockholm</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">Stockholmsgatan</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">Sweden</p>
                </div>
                <div style="text-align: center; font-family: 'Tahoma', Arial, sans-serif;">
                  <h3 style="font-size: 16px; margin-bottom: 8px; color: #333; font-family: 'Tahoma', Arial, sans-serif;">Delivery Address</h3>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{deliveryAddress}}</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{model!.Shipping.CustomerDeliveryInformation.StreetAddress}}</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{model.Shipping.CustomerDeliveryInformation.PostalCode}} {{model.Shipping.CustomerDeliveryInformation.City}}</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{phoneNumber}}</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{model.Shipping.CustomerDeliveryInformation.Country}}</p>
                </div>
                <div style="text-align: center; font-family: 'Tahoma', Arial, sans-serif;">
                  <h3 style="font-size: 16px; margin-bottom: 8px; color: #333; font-family: 'Tahoma', Arial, sans-serif;">Billing Address</h3>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{model!.Shipping.CustomerDeliveryInformation.FullName}}</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{model!.Shipping.CustomerDeliveryInformation.StreetAddress}}</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{model.Shipping.CustomerDeliveryInformation.PostalCode}} {{model.Shipping.CustomerDeliveryInformation.City}}</p>
                  <p style="color: rgba(0,0,0,0.7); font-size: 14px; font-family: 'Tahoma', Arial, sans-serif;">{{model.Shipping.CustomerDeliveryInformation.Country}}</p>
                </div>
              </div>

              <hr style="border: none; border-top: 1px solid #e5e5e5; margin: 24px 0;">

              <div style="font-family: 'Tahoma', Arial, sans-serif;">
                <p style="color: rgba(0,0,0,0.7); font-size: 14px; text-align: center; font-family: 'Tahoma', Arial, sans-serif;">Rika - It will arrive, eventually!</p>
                <p style="color: rgba(0,0,0,0.7); font-size: 14px; text-align: center; font-family: 'Tahoma', Arial, sans-serif;">We'd love your feedback! <a href="" style="color: #000; text-decoration: underline; font-family: 'Tahoma', Arial, sans-serif;">Leave a review</a>.</p>
              </div>
            </div>
            </body>
            </html>
            """;




            var temp2 = $"""
                        <div style="font-family: Tahoma, Arial, sans-serif; margin: 0; padding: 0; background: #f9f9f9;">
              <!-- Container Table -->
              <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center" style="background: #f9f9f9; padding: 24px 0;">
                <tr>
                  <td align="center">
                    <!-- Main Content Table -->
                    <table width="750" cellpadding="0" cellspacing="0" border="0" style="background: #fff; border: 1px solid #e5e5e5; border-radius: 12px; overflow: hidden; font-family: Tahoma, Arial, sans-serif; padding: 0;">
                      <!-- Header -->
                      <tr>
                        <td align="center" style="padding: 36px 24px;">
                          <h1 style="margin: 0; font-family: Tahoma, Arial, sans-serif; font-size: 24px; font-weight: bold;">Here it comes! Your order has been shipped.</h1>
                        </td>
                      </tr>
                      <!-- Button -->
                      <tr>
                        <td align="center" style="padding: 24px;">
                          <a href="" style="display: inline-block; text-decoration: none; background: #000; color: #fff; padding: 12px 24px; border-radius: 24px; font-family: Tahoma, Arial, sans-serif; font-size: 16px; text-align: center;">Track package</a>
                        </td>
                      </tr>
                      <!-- Product Details -->

                            {builder}

                      <!-- Total -->
                      <tr>
                        <td style="padding: 24px; text-align: right;">
                          <h3 style="margin: 0; font-size: 18px; font-weight: bold;">Total: ${model!.OrderTotal}</h3>
                        </td>
                      </tr>
                      <!-- View Receipt Button -->
                      <tr>
                        <td align="center" style="padding: 24px;">
                          <a href="{model.Invoice.InvoiceUrl}" style="display: inline-block; text-decoration: none; background: #000; color: #fff; padding: 12px 24px; border-radius: 24px; font-family: Tahoma, Arial, sans-serif; font-size: 16px; text-align: center;">View Receipt</a>
                        </td>
                      </tr>
                      <!-- Shipping and Billing -->
                      <tr>
                        <td style="padding: 24px;">
                          <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                              <!-- Shipping From -->
                              <td style="text-align: center; width: 33%; font-family: Tahoma, Arial, sans-serif;">
                                <h3 style="margin: 0 0 8px; font-size: 16px; color: #333;">Shipping From</h3>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">Rika Centrallager</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">12345 Stockholm</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">Stockholmsgatan</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">Sweden</p>
                              </td>
                              <!-- Delivery Address -->
                              <td style="text-align: center; width: 33%; font-family: Tahoma, Arial, sans-serif;">
                                <h3 style="margin: 0 0 8px; font-size: 16px; color: #333;">Delivery Address</h3>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{deliveryAddress}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model!.Shipping.CustomerDeliveryInformation.StreetAddress}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model.Shipping.CustomerDeliveryInformation.PostalCode} {model.Shipping.CustomerDeliveryInformation.City}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{phoneNumber}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model.Shipping.CustomerDeliveryInformation.Country}</p>
                              </td>
                              <!-- Billing Address -->
                              <td style="text-align: center; width: 33%; font-family: Tahoma, Arial, sans-serif;">
                                <h3 style="margin: 0 0 8px; font-size: 16px; color: #333;">Billing Address</h3>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model!.Shipping.CustomerDeliveryInformation.FullName}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model!.Shipping.CustomerDeliveryInformation.StreetAddress}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model.Shipping.CustomerDeliveryInformation.PostalCode} {model.Shipping.CustomerDeliveryInformation.City}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model.Shipping.CustomerDeliveryInformation.Country}</p>
                                <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">{model.Shipping.CustomerDeliveryInformation.PhoneNumber}</p>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <!-- Footer -->
                      <tr>
                        <td style="padding: 24px; text-align: center;">
                          <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">Rika - It will arrive, eventually!</p>
                          <p style="margin: 4px 0 0; color: rgba(0,0,0,0.7); font-size: 14px;">We'd love your feedback! <a href="" style="color: #000; text-decoration: underline;">Leave a review</a>.</p>
                        </td>
                      </tr>
                        <tr>
                        <td style="padding: 24px; text-align: center;">
                          <p style="margin: 0; color: rgba(0,0,0,0.7); font-size: 14px;">If you have any issues, please contact our <a href="" style="color: #000; text-decoration: underline;">Customer Support</a></p>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </div>```

            """;




            //var emailBody = $$"""
            //<!DOCTYPE html>
            //<html>
            //<head>
            //</head>
            //<body style="font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f9f9f9;">
            //    <div style="max-width: 900px; margin: 20px auto; background: #ffffff; border: 1px solid #dddddd; padding: 20px; text-align: center;">
            //        <h1 style="font-size: 24px; font-weight: bold; color: #333333; margin-bottom: 20px;">Order Confirmation</h1>
            //        <p style="font-size: 16px; color: #555555; margin-bottom: 30px;">Thank you for your order!</p>
            //        <div style="text-align: left; font-size: 14px; color: #333333; line-height: 1.5;">
            //            <h2 style="font-size: 18px; color: #333333; margin-bottom: 10px;">Shipping Information</h2>
            //            <p><strong>Name:</strong> {{model!.Shipping.CustomerDeliveryInformation.FullName}}</p>
            //            <p><strong>Delivery Address:</strong> {{deliveryAddress}}</p>
            //            <p><strong>Phone:</strong> {{phoneNumber}}</p>
            //            <p><strong>Expected Arrival:</strong> {{model.Shipping.OrderArrival}}</p>
            //           {{tracking}}
            //        </div>
            //        <div style="text-align: left; font-size: 14px; color: #333333; line-height: 1.5;">
            //            <h2 style="font-size: 18px; color: #333333; margin-bottom: 10px;">Invoice Information</h2>
            //            <p><strong>Name:</strong> {{model.Invoice.FullName}}</p>
            //            <p><strong>Address:</strong> {{model.Invoice.StreetAddress}}, {{model.Invoice.City}}, {{model.Invoice.PostalCode}}, {{model.Invoice.Country}}</p>
            //            <p><strong>Payment Method:</strong> {{model.Invoice.PaymentOption}}</p>
            //        </div>
            //        <div style="text-align: left; font-size: 14px; color: #333333; line-height: 1.5;">
            //            <h2 style="font-size: 18px; color: #333333; margin-bottom: 10px;">Order Summary</h2>
            //            <table style="width: 100%; border-collapse: collapse; margin: 20px 0;">
            //                <thead>
            //                    <tr>
            //                        <th style="border: 1px solid #dddddd; max-width: 150px; padding: 10px; text-align: left; background-color: #f2f2f2;"></th>
            //                        <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Product</th>
            //                        <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Amount</th>
            //                        <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Price</th>
            //                        <th style="border: 1px solid #dddddd; padding: 10px; text-align: left; background-color: #f2f2f2;">Discounted Price</th>
            //                    </tr>
            //                </thead>
            //                <tbody>
            //                 {{builder}}
            //                </tbody>
            //            </table>
            //            <p style = "text-align: right; font-size: 16px; font-weight: bold;" > Order Total: {{model.OrderTotal:C}}</p>
            //        </div>
            //        <p style = "margin-top: 30px; font-size: 12px; color: #777777;" > Rika – It will arrive, eventually! </p>
            //    </div>
            //</body>
            //</html>
            //""";

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@beb6ca96-5cd4-43a4-a4bc-6723d17f26d9.azurecomm.net",
                recipientAddress: model.ReceivingEmail,
                content: new EmailContent("Order Confirmation")
                {
                    Html = temp2
                }
            );

            var sendOperation = _client.Send(WaitUntil.Completed, emailMessage);
        }

    }
}
