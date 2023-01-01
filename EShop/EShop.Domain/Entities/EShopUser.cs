using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.Entities
{
    public class EShopUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Password { get; set; } = null!;

        [Required]
        public DateTime RegisterDate { get; set; }

        public bool IsAdmin { get; set; }


        public ICollection<Order> Orders { get; set; } = null!;

        public ICollection<ProductToWishList> WishList { get; set; } = null!;
    }
}
