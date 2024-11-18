using System.ComponentModel.DataAnnotations;

namespace EmailProvider.Models.OrderConfirmationModels
{
    public class ProductModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be atleast 2 characters.")]
        public string Name { get; set; } = null!;
        [Required]
        [Range(1, 10_000)]
        public int Amount { get; set; }
        [Required]
        [Range(1, 100_000)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 100_000)]
        public decimal? DiscountedPrice { get; set; }
        [MinLength(2, ErrorMessage = "ImageUrl must be atleast 2 characters.")]
        [Url]
        public string? ImageUrl { get; set; } 
    }
}
