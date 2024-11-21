using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class PostalAgentDeliveryInformation : DeliveryInformationBase
    {
        [Required]
        public string PostalAgentName { get; set; } = null!;

    }
}
