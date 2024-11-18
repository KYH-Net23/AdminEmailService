using Azure.Communication.Email;
using Azure;
using System.Text;

namespace EmailProvider.EmailServices
{
    public class OrderEmailService
    {
        public void SendOrderConfirmation(OrderConfirmationModel model, string connectionString)
        {
            var emailClient = new EmailClient(connectionString);
            var customerName = model.ExtraContent as string;

            // Bygg HTML för produkttabellen
            var productTableRows = new StringBuilder();
            foreach (var product in model.LineItems)
            {
                productTableRows.Append($@"
            <tr>
                <td style='padding: 10px; font-size: 14px; color: #555;'>{product.Description}</td>
                <td style='padding: 10px; font-size: 14px; color: #555;'>{product.Quantity}</td>
                <td style='padding: 10px; font-size: 14px; color: #555;'>{model.Currency.ToUpper()} {product.Quantity / 100:F2}</td>
            </tr>
        ");
            }

            // Skapa email-meddelandet
            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@beb6ca96-5cd4-43a4-a4bc-6723d17f26d9.azurecomm.net",
                recipientAddress: model.Receiver,
                content: new EmailContent($"Welcome {customerName}")
                {
                    Html = $"""
                <html>
                    <body style='font-family: Montserrat, sans-serif;'>  
                        <table role='presentation' style='width: 100%; max-width: 600px; margin: 0 auto; border-spacing: 0;'>
                            <tr>
                                <td>
                                    <h1 style='margin: 0; padding: 0;'>RIKA</h1>
                                    <h2 style='margin: 0; padding: 0;'>Online Shopping</h2>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding-top: 20px;'>
                                    <h1>Your Payment was Successful</h1>
                                    <p>
                                        Hey {model.CustomerEmail}, <br />
                                        Thank you for your payment! Here are your order details:
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h3>Order Information</h3>
                                    <p>
                                        Payment date: <strong>{model.PaymentDate}</strong><br />
                                        Order number: <strong>{model.OrderId}</strong><br />
                                        Payment method: <strong>{model.PaymentMethodType} {model.PaymentMethodInfo}</strong><br />
                                        Delivery method: <strong>Some delivery method</strong><br />
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td><hr /></td>
                            </tr>
                            <tr>
                                <td>
                                    <h3>Customer Information</h3>
                                    <p>
                                        {model.CustomerName ?? "Name not available"}<br />
                                        {model.AddressLine1 ?? "Address not available"}<br />
                                        {model.PostalCode ?? "Postal code not available"}, {model.City ?? "City not available"}<br />
                                        {model.Country ?? "Country not available"}<br />
                                        {model.PhoneNumber ?? "Phone number not available"}<br />
                                        {model.CustomerEmail ?? "Email not available"}<br />
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td><hr /></td>
                            </tr>
                            <tr>
                                <td>
                                    <h3>Products</h3>
                                    <table role='presentation' style='width: 100%; max-width: 600px; border-spacing: 0; margin: 0 auto; padding: 0;'>
                                        <thead>
                                            <tr>
                                                <th style='text-align: left; font-size: 14px; color: #333; border-bottom: 1px solid #ddd;'>Product</th>
                                                <th style='text-align: left; font-size: 14px; color: #333; border-bottom: 1px solid #ddd;'>Quantity</th>
                                                <th style='text-align: left; font-size: 14px; color: #333; border-bottom: 1px solid #ddd;'>Price</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {productTableRows}
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td><hr /></td>
                            </tr>
                            <tr>
                                <td>
                                    <h3>Order Total</h3>
                                    <p>
                                        Subtotal: {model.Currency} {model.AmountSubtotal / 100:F2}<br />
                                          Shipping: {model.Currency} {(model.ShippingCost > 0 ? model.ShippingCost / 100.0 : 0.0):F2}<br />
                                          Tax: {model.Currency} {(model.Tax > 0 ? model.Tax / 100.0 : 0.0):F2}<br />
                                        Total: {model.Currency} {model.AmountTotal / 100:F2}<br />
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p>
                                        Thank you, <br />
                                        Rika
                                    </p>   
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>    
            """
                }
            );

            // Skicka e-post
            EmailSendOperation emailSendOperation = emailClient.Send(
                WaitUntil.Completed,
                emailMessage
            );
        }
    }
}
