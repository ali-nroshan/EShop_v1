using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using EShop.Domain.Entities;
using EShop.Domain.Interfaces;

namespace EShop.Core.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository categoryRepository;
		private readonly IStorageService storageService;

		public CategoryService(ICategoryRepository categoryRepository,
			IStorageService storageService)
		{
			this.categoryRepository = categoryRepository;
			this.storageService = storageService;
		}

		public void AddCategory(string categoryName, byte[] categoryLogo, string extension)
		{
			if (!storageService.CheckFileExtension(extension))
				throw new Exception("Extension is not from safe list, Status Code = 401");
			string categoryLogoName = storageService
				.GenerateRandomFileNameWithExtension(extension);
			StoreCategoryLogo(categoryLogoName, categoryLogo);
			categoryRepository.AddCategory(categoryName, categoryLogoName);
		}

		public List<CategoryViewModel> GetCategories()
		{
			return categoryRepository.GetCategories()
				.Select(q => new CategoryViewModel()
				{
					CategoryId = q.Id,
					CategoryName = q.Name
				}).ToList();
		}

		public int GetCategoryProductsCount(int categoryId)
		{
			return categoryRepository.GetCategoryProductsCount(categoryId);
		}

		public List<CategoryWithLogoViewModel> GetCategoryWithLogos()
		{
			return categoryRepository.GetCategories()
				.Select(q => new CategoryWithLogoViewModel()
				{
					CategoryId = q.Id,
					CategoryName = q.Name,
					SubProductsCount = q.CategoryToProducts.Count,
					CategoryLogoName = q.CategoryLogoName
				}).ToList();
		}

		public void RemoveCategory(int categoryId)
		{
			var category = categoryRepository.GetCategoryById(categoryId);
			if (category is null)
				throw new Exception("RemoveCategory() in CategoryService, Status Code = 404");
			RemoveCategoryLogo(category.CategoryLogoName);
			categoryRepository.RemoveCategory(category);
		}

		public void UpdateCategoryName(int categoryId, string categoryName)
		{
			var category = categoryRepository.GetCategoryById(categoryId);
			if (category is null)
				throw new Exception("UpdateCategoryName() in CategoryService, Status Code = 404");
			category.Name = categoryName;
			categoryRepository.UpdateCategory(category);
		}

		public void UpdateCategoryLogo(int categoryId, byte[] categoryLogo, string extension)
		{
			if (!storageService.CheckFileExtension(extension))
				throw new Exception("Extension is not from safe list, Status Code = 401");

			var category = categoryRepository.GetCategoryById(categoryId);
			if (category is null)
				throw new Exception("UpdateCategoryLogo() in CategoryService, Status Code = 404");
			RemoveCategoryLogo(category.CategoryLogoName);
			category.CategoryLogoName = Path.ChangeExtension(category.CategoryLogoName, extension);
			StoreCategoryLogo(category.CategoryLogoName, categoryLogo);
			categoryRepository.UpdateCategory(category);
		}

		private void StoreCategoryLogo(string categoryLogoName, byte[] data)
		{
			if (!storageService.StoreFile(data, categoryLogoName, StoreFileType.CategoryImage))
				throw new Exception("StoreCategoryLogo() in CategoryService, Status Code = 500");
		}

		private void RemoveCategoryLogo(string categoryLogoName)
		{
			if (!storageService.RemoveFile(categoryLogoName, StoreFileType.CategoryImage))
				throw new Exception("RemoveCategoryLogo() in CategoryService, Status Code = 500");
		}

		public decimal GetCategoryTotalSale(int categoryId)
		{
			return categoryRepository.GetCategoryTotalSale(categoryId);
		}

		public List<Product> GetCategoryProducts(int categoryId)
		{
			return categoryRepository.GetCategoryProducts(categoryId).ToList();
		}

		public CategoryViewModel? GetCategoryById(int categoryId)
		{
			var category = categoryRepository.GetCategoryById(categoryId);
			return (category != null) ? new CategoryViewModel()
			{
				CategoryId = categoryId,
				CategoryName = category.Name
			} : null;
		}

		public CategoryWithLogoViewModel? GetCategoryWithLogoById(int categoryId)
		{
			var category = categoryRepository.GetCategoryById(categoryId);
			return (category != null) ? new CategoryWithLogoViewModel()
			{
				CategoryId = categoryId,
				CategoryName = category.Name,
				CategoryLogoName = category.CategoryLogoName
			} : null;
		}
	}
}
