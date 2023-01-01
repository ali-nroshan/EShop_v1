using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models
{
	public class CategoryViewModel
	{
		[Required]
		public int CategoryId { get; set; }

		public IFormFile? CategoryLogo { get; set; }

		[Required, MaxLength(50)]
		public string CategoryName { get; set; } = null!;
	}

	public class CategoryReportViewModel
	{
		public int CategoryId { get; set; }

		public string CategoryName { get; set; } = null!;

		public List<ProductSummaryViewModel> Products { get; set; }
		= new List<ProductSummaryViewModel>();
	}

	public class CategorySummaryViewModel
	{
		public int CategoryId { get; set; }
		public decimal CategoryTotalSale { get; set; }
		public string CategoryName { get; set; } = null!;
		public int SubProductsCount { get; set; }
		public string CategoryLogo { get; set; } = null!;
	}
}
