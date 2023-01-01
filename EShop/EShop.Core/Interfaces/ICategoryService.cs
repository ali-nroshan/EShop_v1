using EShop.Core.ViewModels;
using EShop.Domain.Entities;

namespace EShop.Core.Interfaces
{
    public interface ICategoryService
    {
        List<Product> GetCategoryProducts(int categoryId);
        CategoryViewModel? GetCategoryById(int categoryId);
        CategoryWithLogoViewModel? GetCategoryWithLogoById(int categoryId);
        List<CategoryWithLogoViewModel> GetCategoryWithLogos();
        List<CategoryViewModel> GetCategories();
        int GetCategoryProductsCount(int categoryId);
        decimal GetCategoryTotalSale(int categoryId);

        void AddCategory(string categoryName, byte[] categoryLogo, string extension);
        void RemoveCategory(int categoryId);
        void UpdateCategoryName(int categoryId, string categoryName);
        void UpdateCategoryLogo(int categoryId, byte[] categoryLogo, string extension);
    }
}
