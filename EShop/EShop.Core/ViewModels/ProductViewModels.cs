using System.ComponentModel.DataAnnotations;

namespace EShop.Core.ViewModels
{
    public class ProductListViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public List<ProductViewModel> Product { get; set; } = new List<ProductViewModel>();
    }
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductImageName { get; set; } = null!;
        public float? ProductScore { get; set; }
        public decimal ProductPrice { get; set; }
        public int QuantityInStock { get; set; }
        public string ProductDescription { get; set; } = null!;
        public List<CategoryViewModel> Categories { get; set; } = null!;
    }

    public class ProductItemViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string ProductImageFile { get; set; } = null!;

        [Required]
        public decimal ProductPrice { get; set; }

        [Required, MinLength(1000)]
        public string ProductDescription { get; set; } = null!;

        public float? ProductScore { get; set; }

        public bool HasInventory { get; set; } = false;
    }
}
