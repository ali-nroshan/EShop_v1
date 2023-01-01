using EShop.Domain.Entities;

namespace EShop.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        int GetCategoryProductsCount(int categoryId);
        IQueryable<Category> GetCategories();
        Category? GetCategoryById(int id);
        IQueryable<Product> GetCategoryProducts(int categoryId);
        decimal GetCategoryTotalSale(int categoryId);
        void AddCategory(string categoryName, string categoryLogoName);
        void RemoveCategory(Category category);
        void UpdateCategory(Category category);
    }
}
