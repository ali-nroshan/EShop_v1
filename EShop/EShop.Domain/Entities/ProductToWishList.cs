using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Entities
{
    public class ProductToWishList
    {
        public int ProductId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public EShopUser EShopUser { get; set; } = null!;

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}
