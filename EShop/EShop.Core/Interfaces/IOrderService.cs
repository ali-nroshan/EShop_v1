using EShop.Core.ViewModels;

namespace EShop.Core.Interfaces
{
    public interface IOrderService
    {
        public bool AddProductToOrder(int userId, int productId, int count);
        public bool FinalizeOrder(int userId);
        public CartViewModel GetUserOrder(int userId);
        public bool RemoveProductFromOrder(int userId, int productId);
    }
}
