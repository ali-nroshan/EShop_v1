using EShop.Domain.Entities;
using EShop.Domain.Interfaces;
using EShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EShopDbContext _context;
        public OrderRepository(EShopDbContext context)
        {
            _context = context;
        }
        public bool AddProductToOrder(int userId, int productId, int count)
        {
            if (!_context.EShopUsers.Any(q => q.Id == userId))
                return false;

            var product = _context.Products.Find(productId);
            if (product == null || count > product.QuantityInStock || count <= 0)
                return false;


            var order = _context.Orders.SingleOrDefault(q => q.UserId == userId && q.IsFinaly == false);
            if (order == null)
            {
                order = new Order
                {
                    CreateDate = DateTime.UtcNow,
                    IsFinaly = false,
                    UserId = userId
                };
                _context.Add(order);
                _context.SaveChanges();
            }

            var orderDetail = _context.OrderDetails.SingleOrDefault(
                q => q.OrderId == order.OrderId && q.ProductId == product.ProductId);
            if (orderDetail == null)
            {
                orderDetail = new  OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductId,
                    Count = count,
                    Price = product.ProductPrice * count
                };
                _context.Add(orderDetail);
            }
            else
            {
                orderDetail.Count = count;
                orderDetail.Price = product.ProductPrice * count;
                _context.Update(orderDetail);
            }
            _context.SaveChanges();

            return true;
        }

        public bool FinalizeOrder(int userId)
        {
            var user = _context.EShopUsers.Include(q => q.Orders)
                .FirstOrDefault(q => q.Id == userId);
            if (user == null) return false;

            var user_order = user.Orders.FirstOrDefault(q => !q.IsFinaly)!;
            if (user_order == null) return false;

            var order_details = _context.Orders
                .Include(q => q.OrderDetails).First(q => q.OrderId == user_order.OrderId)!.OrderDetails;
            if (order_details.Count == 0) return false;

            foreach (var item in order_details)
            {
                var product = _context.Products.Find(item.ProductId)!;
                product.QuantityInStock -= item.Count;
                if (product.QuantityInStock < 0)
                    return false;
                _context.Update(product);
            }

            user_order.IsFinaly = true;
            _context.Update(user_order);
            _context.SaveChanges();
            return true;
        }

        public Order? GetUserOrder(int userId)
        {
			var order = _context.Orders
                .SingleOrDefault(o => o.UserId == userId && !o.IsFinaly);

			if (order is not null)
                order.OrderDetails = _context.OrderDetails
                .Include(q => q.Product).Where(q => q.OrderId == order.OrderId).ToList();
            return order;
        }

        public bool RemoveProductFromOrder(int userId, int productId)
        {
            if (!_context.EShopUsers.Any(q => q.Id == userId))
                return false;

            var product = _context.Products.Find(productId);
            if (product == null)
                return false;
            var order = _context.Orders.Include(q => q.OrderDetails).SingleOrDefault(q => q.UserId == userId && !q.IsFinaly);
            if (order == null)
                return false;
            var orderDetail = order.OrderDetails
                .SingleOrDefault(q => q.ProductId == productId);

            if (orderDetail == null)
                return false;

            _context.OrderDetails.Remove(orderDetail);
            if (order.OrderDetails.Count - 1 == 0)
                _context.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
}
