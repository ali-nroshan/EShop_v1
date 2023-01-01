namespace EShop.Core.ViewModels
{

    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }

    public class CategoryWithLogoViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryLogoName { get; set; } = null!;
        public int SubProductsCount { get; set; }
    }
}
