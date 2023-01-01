using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using EShop.Domain.Entities;
using EShop.Domain.Interfaces;
using EShop.Core.Common.ObjectMapping;

namespace EShop.Core.Services
{
	public class ProductOfferService : IProductOfferService
	{
		private readonly IProductRepository productRepository;
		private readonly ICategoryRepository categoryRepository;
		private const int _maxProductOffer = 20;


		public ProductOfferService(IProductRepository productRepository,
			ICategoryRepository categoryRepository)
		{
			this.productRepository = productRepository;
			this.categoryRepository = categoryRepository;
		}

		private ProductOfferDataViewModel GetProductData(Product product)
		=> new ProductOfferDataViewModel()
		{
			Price = product.ProductPrice,
			ProductId = product.ProductId,
			Score = product.ProductScore ?? 0,
			TotalSale = productRepository.GetProductTotalSale(product.ProductId)
		};

		private List<ProductOfferDataViewModel> GetProductsData(List<Product> products)
			=> products.Select(p => GetProductData(p)).ToList();

		private List<ProductOfferDataViewModel> GetProductsData()
			=> GetProductsData(productRepository.GetProducts().ToList());



		public CategoryBaseOfferViewModel GetCategoryBaseOffers(CategoryBaseOfferOption offerOption)
		{
			var rs = new CategoryBaseOfferViewModel();

			switch (offerOption)
			{
				case CategoryBaseOfferOption.SpecialOffer:
					rs.OfferTypeCode = "SpBeOf";
					rs.OfferTypeName = "محصولات ویژه";
					break;
				case CategoryBaseOfferOption.NewOffer:
					rs.OfferTypeCode = "NwBeOf";
					rs.OfferTypeName = "محصولات جدید";
					break;
				case CategoryBaseOfferOption.TopScoresOffer:
					rs.OfferTypeCode = "TpSsBeOf";
					rs.OfferTypeName = "محصولات با بالاترین امتیاز";
					break;
				default:
					break;
			}

			foreach (var category in categoryRepository.GetCategories().ToList())
			{
				var temp = GetCategoryOffers(category.Id, offerOption);
				if (temp != null)
					rs.OfferItems.Add(temp);
			}

			rs.OfferItems = rs.OfferItems.Take(6).ToList();

			return rs;
		}

		public CategoryBaseOfferItemViewModel GetCategoryOffers(int categoryId, CategoryBaseOfferOption offerBase)
		{
			var category = categoryRepository.GetCategoryById(categoryId);
			if (category == null)
				throw new Exception("Category not found");

			var category_products = categoryRepository.GetCategoryProducts(categoryId).ToList();
			var rs = new CategoryBaseOfferItemViewModel()
			{
				CategoryId = categoryId,
				CategoryName = category.Name
			};

			if (offerBase == CategoryBaseOfferOption.NewOffer)
			{
				category_products = category_products.OrderByDescending(q => q.ProductId)
					.Take(_maxProductOffer).ToList();
			}
			else if (offerBase == CategoryBaseOfferOption.TopScoresOffer)
			{
				category_products =
					category_products.Where(q => q.ProductScore >= 3)
					.OrderByDescending(q => q.ProductScore)
					.Take(_maxProductOffer).ToList();
			}
			else if (offerBase == CategoryBaseOfferOption.SpecialOffer)
			{
				category_products =
					 category_products.Where(q => q.ProductScore >= 3.5)
						.OrderBy(q => q.ProductPrice)
							.Take(_maxProductOffer).ToList();
			}
			else
				throw new Exception();
			rs.ProductItems.AddRange(category_products.Select(q => q.ProductToProductItemVM()));
			return rs;
		}

		public ProductBaseOfferViewModel GetProductBaseOffers(int productId, ProductBaseOfferOption offerBase)
		{
			var product = productRepository.GetProduct(productId);
			if (product == null)
				throw new Exception("product not found");

			var offer = new ProductBaseOfferViewModel();


			if (offerBase == ProductBaseOfferOption.RelativeOffer)
			{
				var relative_products = GetProductRelatives(productId);
				var relative_products_data = GetProductsData(relative_products);
				relative_products_data.Remove(
					relative_products_data.Find(q => q.ProductId == productId)!);
				relative_products_data = relative_products_data
					.OrderByDescending(q => q.TotalSale)
					.Take(_maxProductOffer).ToList();

				relative_products_data
					.ForEach(p =>
					{
						offer.ProductItems.
						Add(relative_products.First(q => q.ProductId == p.ProductId).ProductToProductItemVM());
					});
			}
			else if (offerBase == ProductBaseOfferOption.TopSalesOffer)
			{
				var top_sale_products = GetProductsData();
				var temp = top_sale_products.Find(q => q.ProductId == productId);
				if (temp != null)
					top_sale_products.Remove(temp);
				top_sale_products = top_sale_products
					.OrderByDescending(q => q.TotalSale).Take(_maxProductOffer).ToList();

				top_sale_products.ForEach(p => // check logic
				offer.ProductItems.Add(productRepository.GetProduct(productId)!.ProductToProductItemVM()));
			}

			switch (offerBase)
			{
				case ProductBaseOfferOption.RelativeOffer:
					offer.OfferTypeName = "محصولات مرتبط";
					break;
				case ProductBaseOfferOption.TopSalesOffer:
					offer.OfferTypeName = "پرفروش ترین ها";
					break;
				default:
					break;
			}

			return offer;
		}

		public List<Product> GetProductRelatives(int productId)
		{
			var product_categories = productRepository.GetProductCategories(productId).ToList();
			var rs = new List<Product>();

			product_categories.ForEach(c =>
				rs.AddRange(categoryRepository.GetCategoryProducts(c.Id))
			);

			rs = rs.Distinct().ToList();
			return rs;
		}
	}
}
