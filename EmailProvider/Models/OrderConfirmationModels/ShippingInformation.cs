using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class ShippingInformation
    {
        [Required]
        [MinLength(2, ErrorMessage = "Full Name must be atleast 2 characters.")]
        public string FullName { get; set; } = null!;
        [Required]
        [MinLength(2, ErrorMessage = "Customer Delivery Address must be atleast 2 characters.")]
        public string CustomerDeliveryAddress { get; set; } = null!;
        [Required]
        [MinLength(2, ErrorMessage = "Postal Pickup Address must be atleast 2 characters.")]
        public string? PostalPickUpAddress { get; set; }
        [RegularExpression(@"^\+46\d{9}$", ErrorMessage = "Phone number must start with +46 and be followed by exactly 9 digits.")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [FutureDate]
        public DateOnly OrderArrival { get; set; }
        [MinLength(2, ErrorMessage = "Tracking Link must be atleast 2 characters.")]
        public Uri? TrackingLink { get; set; }
    }
}
