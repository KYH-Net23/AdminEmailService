using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class CustomerDeliveryInformation : DeliveryInformationBase
    {
        [Required]
        [MinLength(2, ErrorMessage = "Full Name must be atleast 2 characters.")]
        public string FullName { get; set; } = null!;
    }
}