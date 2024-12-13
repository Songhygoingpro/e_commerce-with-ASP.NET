using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? Image { get; set; }
    }

}
