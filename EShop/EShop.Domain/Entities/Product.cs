using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(300)]
        public string ProductName { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string ProductImageFileName { get; set; } = null!;

        [Required, MaxLength(3000)]
        public string ProductDescription { get; set; } = null!;

        [Required]
        public int QuantityInStock { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        public float? ProductScore { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
        public ICollection<CategoryToProduct> CategoryToProducts { get; set; } = null!;
        public ICollection<ProductToWishList> ProductToWishList { get; set; } = null!;
    }
}
