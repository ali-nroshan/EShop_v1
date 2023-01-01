using EShop.Core.Interfaces;

namespace EShop.Infrastructure.Common
{
	public class StorageService : IStorageService
	{
		private string _productFolder, _categoryFolder;
		public List<string> SafeExtensions { get; init; }

		public StorageService()
		{
			SafeExtensions = new List<string>()
			{
				".png",
				".jpg"
			};
			_productFolder = Path.Combine(Directory.GetCurrentDirectory(),
				"wwwroot", "assets", "img", "product");
			_categoryFolder = Path.Combine(Directory.GetCurrentDirectory(),
				"wwwroot", "assets", "img", "category");
		}

		#region Utilities
		private string GetProductImagePath(string productImageName)
			=> Path.Combine(_productFolder, productImageName);

		private string GetCategoryLogoPath(string categoryLogoName)
			=> Path.Combine(_categoryFolder, categoryLogoName);
		#endregion

		public bool CheckFileExtension(string fileName)
		{
			string extension = Path.GetExtension(fileName);
			return SafeExtensions.Any(q => q == extension);
		}

		public string GenerateRandomFileNameWithExtension(string extension)
			=> Guid.NewGuid().ToString() + extension;

		public bool RemoveFile(string fileName, StoreFileType storeFileType)
		{
			string filePath = (storeFileType == StoreFileType.ProductImage)
				? GetProductImagePath(fileName)
				: GetCategoryLogoPath(fileName);
			try
			{
				File.Delete(filePath);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool StoreFile(byte[] data, string fileName, StoreFileType storeFileType)
		{
			string filePath = (storeFileType == StoreFileType.ProductImage)
				? GetProductImagePath(fileName)
				: GetCategoryLogoPath(fileName);
			try
			{
				File.WriteAllBytes(filePath, data);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
