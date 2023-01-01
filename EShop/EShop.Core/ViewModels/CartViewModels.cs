using System.ComponentModel.DataAnnotations;

namespace EShop.Core.ViewModels
{
    public record UpdateCartViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }
    }

    public record CartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        public decimal TotalPrice { get; set; }
    }

    public record CartItemViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int ProductQuantity { get; set; }

        public decimal PriceSum { get; set; }

        public int ProductCountForCart { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImage { get; set; } = null!;
    }


}
