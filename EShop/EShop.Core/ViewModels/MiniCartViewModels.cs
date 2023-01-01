namespace EShop.Core.ViewModels
{
    public class MiniCartViewModel
    {
        public List<MiniCartItemViewModel> MiniCartItems { get; set; } = new List<MiniCartItemViewModel>();

        public decimal TotalPrice { get; set; }
    }

    public class MiniCartItemViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int ProductCount { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal TotoalPrice { get; set; }

        public string ProductImage { get; set; } = null!;
    }
}
