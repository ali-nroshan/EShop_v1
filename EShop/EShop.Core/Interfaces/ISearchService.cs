using EShop.Domain.Entities;

namespace EShop.Core.Interfaces
{
    public interface ISearchService
    {
        List<Product> SearchProducts(string searchTerm, int? categoryId);
        List<Product> GetCategoryProducts(int categoryId, ref string categoryName);
    }
}