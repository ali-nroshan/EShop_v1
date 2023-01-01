using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        public bool IsFinaly { get; set; }


        [ForeignKey("UserId")]
        public EShopUser EShopUser { get; set; } = null!;
        public List<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
