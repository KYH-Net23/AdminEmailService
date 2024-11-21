using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class InvoiceInformation : DeliveryInformationBase
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be atleast 2 characters.")]
        public string FullName { get; set; } = null!;      
        [Required]
        [MinLength(2, ErrorMessage = "Payment Option must be atleast 2 characters.")]
        public string PaymentOption { get; set; } = null!;
        [Url]
        public string? InvoiceUrl { get; set; }
    }
}
