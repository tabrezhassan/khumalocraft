using System.ComponentModel.DataAnnotations;

namespace khumalocraft.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }


        public string? ImageUrl { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Availability must be greater than or equal to 0")]
        public int Availability { get; set; }
    }
}