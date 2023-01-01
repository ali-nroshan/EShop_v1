using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using EShop.Domain.Interfaces;

namespace EShop.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public bool AddProductToOrder(int userId, int productId, int count)
        {
            return orderRepository.AddProductToOrder(userId, productId, count);
        }

        public bool FinalizeOrder(int userId)
        {
            return orderRepository.FinalizeOrder(userId);
        }

        public CartViewModel GetUserOrder(int userId)
        {
            var order = orderRepository.GetUserOrder(userId);
            if (order == null)
                return null!;


            var cartm = new CartViewModel();

            cartm.CartItems = order.OrderDetails
                .Select(q => new CartItemViewModel()
                {
                    ProductId = q.ProductId,
                    ProductImage = q.Product.ProductImageFileName,
                    ProductCountForCart = q.Count,
                    PriceSum = q.Count * q.Product.ProductPrice,
                    ProductPrice = q.Product.ProductPrice,
                    ProductName = q.Product.ProductName,
                    ProductQuantity = q.Product.QuantityInStock
                }).ToList();
            cartm.TotalPrice = cartm.CartItems.Sum(q => q.PriceSum);

            return cartm;
        }

        public bool RemoveProductFromOrder(int userId, int productId)
        {
            return orderRepository.RemoveProductFromOrder(userId, productId);
        }
    }
}
