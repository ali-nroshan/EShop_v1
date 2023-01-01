using EShop.Core.ViewModels;

namespace EShop.Core.Interfaces
{
    public interface IProductService
    {
        ProductViewModel? GetProduct(int productId);
        List<CategoryViewModel> GetProductCategories(int productId);
        List<ProductViewModel> GetProducts();
        decimal GetProductTotalSale(int productId);

        void AddProduct(ProductViewModel productVM, byte[] productImage, string extension);
        void RemoveProduct(int productId);
        void UpdateProductImage(int productId, byte[] productImage, string extension);
        void UpdateProduct(ProductViewModel product);
        void SetProductCategories(int productId, List<int> categoriesId);
        void RemoveProductCategories(int productId, List<int> categoriesId);
    }
}
