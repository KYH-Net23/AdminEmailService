using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class ShippingInformation
    {
        [Required]
        public CustomerDeliveryInformation CustomerDeliveryInformation { get; set; } = null!;
        public PostalAgentDeliveryInformation? PostalAgentDeliveryInformation { get; set;}
        [Required]
        [FutureDate]
        public DateOnly OrderArrival { get; set; }
        [MinLength(2, ErrorMessage = "Tracking Link must be atleast 2 characters.")]
        [Url]
        public string? TrackingLink { get; set; }
    }
}
