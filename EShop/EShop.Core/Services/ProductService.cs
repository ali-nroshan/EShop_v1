using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using EShop.Domain.Entities;
using EShop.Domain.Interfaces;

namespace EShop.Core.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository productRepository;
		private readonly IStorageService storageService;

		public ProductService(IProductRepository productRepository,
			IStorageService storageService)
		{
			this.productRepository = productRepository;
			this.storageService = storageService;
		}

		public ProductViewModel? GetProduct(int productId)
		{
			var product = productRepository.GetProduct(productId);
			var rp = ProductToProductVM(product);
			if (rp!=null)
			{
				rp.Categories = productRepository.GetProductCategories(productId)
					.Select(q => new CategoryViewModel()
					{
						CategoryId = q.Id,
						CategoryName = q.Name
					}).ToList();
			}
			return rp;
		}

		public List<CategoryViewModel> GetProductCategories(int productId)
		{
			return productRepository.GetProductCategories(productId)
				.Select(q => new CategoryViewModel()
				{
					CategoryId = q.Id,
					CategoryName = q.Name
				}).ToList();
		}

		private static ProductViewModel? ProductToProductVM(Product? product)
		{
			return (product != null) ?
				new ProductViewModel()
				{
					ProductId = product.ProductId,
					ProductDescription = product.ProductDescription,
					ProductImageName = product.ProductImageFileName,
					ProductName = product.ProductName,
					ProductPrice = product.ProductPrice,
					ProductScore = product.ProductScore,
					QuantityInStock = product.QuantityInStock
				}
				: null;
		}

		public List<ProductViewModel> GetProducts()
		{
			return productRepository.GetProducts()
				.Select(q => ProductToProductVM(q)!).ToList();
		}

		public decimal GetProductTotalSale(int productId)
		{
			return productRepository.GetProductTotalSale(productId);
		}


		//------------------------------------------------------------------

		private void StoreProductImage(string productImageName, byte[] data)
		{
			if (!storageService
				.StoreFile(data, productImageName, StoreFileType.ProductImage))
				throw new Exception("StoreProductImage() in ProductService, Status Code = 500");
		}

		private void RemoveProductImage(string productImageName)
		{
			if (!storageService.RemoveFile(productImageName,
				StoreFileType.ProductImage))
				throw new Exception("RemoveCategoryLogo() in ProductService, Status Code = 500");
		}



		public void AddProduct(ProductViewModel productVM, byte[] productImage, string extension)
		{
			if (!storageService.CheckFileExtension(extension))
				throw new Exception("Extension is not from safe list, Status Code = 401");
			string productImageName = storageService.GenerateRandomFileNameWithExtension(extension);
			StoreProductImage(productImageName, productImage);
			var product = new Product()
			{
				ProductDescription = productVM.ProductDescription,
				ProductImageFileName = productImageName,
				ProductName = productVM.ProductName,
				ProductPrice = productVM.ProductPrice,
				ProductScore = productVM.ProductScore,
				QuantityInStock = productVM.QuantityInStock
			};
			productRepository.AddProduct(product);
			productVM.ProductId = product.ProductId;
		}

		public void RemoveProduct(int productId)
		{
			var product = productRepository.GetProduct(productId);
			if (product == null)
				throw new Exception("RemoveProduct() in ProductService 404");
			RemoveProductImage(product.ProductImageFileName);
			productRepository.RemoveProduct(product);
		}

		public void UpdateProductImage(int productId, byte[] productImage, string extension)
		{
			if (!storageService.CheckFileExtension(extension))
				throw new Exception("Extension is not from safe list, Status Code = 401");
			var product = productRepository.GetProduct(productId);
			if (product == null)
				throw new Exception("RemoveProduct() in ProductService 404");
			RemoveProductImage(product.ProductImageFileName);
			product.ProductImageFileName = Path.ChangeExtension(product.ProductImageFileName, extension);
			StoreProductImage(product.ProductImageFileName,productImage);
			productRepository.UpdateProduct(product);
		}

		public void UpdateProduct(ProductViewModel productVm)
		{
			var product = productRepository.GetProduct(productVm.ProductId);
			if (product == null)
				throw new Exception("RemoveProduct() in ProductService 404");

			product.ProductDescription = productVm.ProductDescription;
			product.ProductScore = productVm.ProductScore;
			product.ProductPrice = productVm.ProductPrice;
			product.ProductName = productVm.ProductName;
			product.QuantityInStock = productVm.QuantityInStock;
			productRepository.UpdateProduct(product);
		}

		public void SetProductCategories(int productId, List<int> categoriesId)
		{
			productRepository.SetProductCategories(productId, categoriesId);
		}

		public void RemoveProductCategories(int productId, List<int> categoriesId)
		{
			productRepository.RemoveProductCategories(productId, categoriesId);
		}
	}
}
