using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class InvoiceInformation
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be atleast 2 characters.")]
        public string FullName { get; set; } = null!;
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
        [Required]
        [MinLength(2, ErrorMessage = "Payment Option must be atleast 2 characters.")]
        public string PaymentOption { get; set; } = null!;
    }
}
