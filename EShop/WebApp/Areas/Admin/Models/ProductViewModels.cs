using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models
{
	public class ProductViewModel
	{
		[Required]
		public int ProductId { get; set; }

		[Required, MaxLength(300)]
		public string ProductName { get; set; } = null!;

		[Required]
		public decimal ProductPrice { get; set; }

		public IFormFile? ProductImage { get; set; }

		[Required, MaxLength(3000)]
		public string ProductDescription { get; set; } = null!;

		[Required]
		public int QuantityInStock { get; set; }

		public float? ProductScore { get; set; }

		public List<int> SelectedCategories { get; set; } = null!;
	}

	public class ProductSummaryViewModel
	{
		public int ProductId { get; set; }

		public string ProductName { get; set; } = null!;

		public string ProductImageFile { get; set; } = null!;

		public bool InventoryState { get; set; }

		public float? ProductScore { get; set; }
	}
}
