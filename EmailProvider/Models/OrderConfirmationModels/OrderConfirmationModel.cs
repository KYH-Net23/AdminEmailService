using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class OrderConfirmationModel
    {
        public string EmailType { get; set; } = "Order";
        [Required]
        public string ReceivingEmail { get; set; } = null!;
        [Required]
        public ShippingInformation Shipping { get; set; } = null!;
        [Required]
        public InvoiceInformation Invoice { get; set; } = null!;
        [Required]
        public List<ProductModel> Products { get; set; } = null!;
        [Required]
        [Range(1, 10_000_000)]
        public decimal OrderTotal { get; set; }

    }
}
