using EShop.Domain.Entities;

namespace EShop.Domain.Interfaces
{
    public interface IUserWishListRepository
    {
        IQueryable<Product> GetUserWishList(int userId);

        bool AddProductToWishList(int userId, int productId);

        bool RemoveProductFromWishList(int userId, int productId);

        int GetWishListProductCount(int userId);
    }
}
