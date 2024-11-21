using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class DeliveryInformationBase
    {
        [Required]
        [MinLength(2, ErrorMessage = "Address must be atleast 2 characters.")]
        public string StreetAddress { get; set; } = null!;
        [Required]
        [MinLength(2, ErrorMessage = "City must be atleast 2 characters.")]
        public string City { get; set; } = null!;
        [Required]
        [StringLength(5)]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Please enter exactly 5 digits.")]
        public string PostalCode { get; set; } = null!;
        [Required]
        [MinLength(2, ErrorMessage = "Country must be atleast 2 characters.")]
        public string Country { get; set; } = null!;
        [RegularExpression(@"^\+46\d{9}$", ErrorMessage = "Phone number must start with +46 and be followed by exactly 9 digits.")]
        public string PhoneNumber { get; set; } = null!;
    }
}
