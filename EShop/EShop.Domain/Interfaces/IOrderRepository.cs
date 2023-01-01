using EShop.Domain.Entities;

namespace EShop.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Order? GetUserOrder(int userId);

        bool AddProductToOrder(int userId, int productId, int count);

        bool RemoveProductFromOrder(int userId, int productId);

        bool FinalizeOrder(int userId);
    }
}
