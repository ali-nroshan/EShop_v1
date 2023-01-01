using EShop.Core.ViewModels;
using EShop.Domain.Entities;

namespace EShop.Core.Interfaces
{
    public interface IProductOfferService
    {
        List<Product> GetProductRelatives(int productId);
        ProductBaseOfferViewModel GetProductBaseOffers(int productId, ProductBaseOfferOption offerBase);
        CategoryBaseOfferItemViewModel GetCategoryOffers(int categoryId, CategoryBaseOfferOption offerBase);
        CategoryBaseOfferViewModel GetCategoryBaseOffers(CategoryBaseOfferOption offerOption);
    }
}