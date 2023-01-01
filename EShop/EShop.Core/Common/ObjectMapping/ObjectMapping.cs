using EShop.Core.ViewModels;
using EShop.Domain.Entities;

namespace EShop.Core.Common.ObjectMapping
{
	public static partial class ObjectMapping
	{
		public static ProductItemViewModel ProductToProductItemVM(this Product product)
		=> new ProductItemViewModel()
		{
			HasInventory = product.QuantityInStock > 0,
			ProductDescription = product.ProductDescription,
			ProductId = product.ProductId,
			ProductImageFile = product.ProductImageFileName,
			ProductName = product.ProductName,
			ProductPrice = product.ProductPrice,
			ProductScore = product.ProductScore ?? 0
		};
	}
}
