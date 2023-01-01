using EShop.Domain.Entities;

namespace EShop.Domain.Interfaces
{
    public interface IProductRepository
    {
        Product? GetProduct(int productId);
        IQueryable<Category> GetProductCategories(int productId);
        IQueryable<Product> GetProducts();
        decimal GetProductTotalSale(int productId);

        void AddProduct(Product product);
        void RemoveProduct(Product product);
        void UpdateProduct(Product product);
        void SetProductCategories(int productId,List<int> categories);
        void RemoveProductCategories(int productId, List<int> categories);
    }
}
