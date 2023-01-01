using EShop.Domain.Entities;
using EShop.Domain.Interfaces;
using EShop.Infrastructure.Context;

namespace EShop.Infrastructure.Repositories
{
    public class UserWishListRepository : IUserWishListRepository
    {
        private readonly EShopDbContext _context;

        public UserWishListRepository(EShopDbContext context)
        {
            _context = context;
        }

        public bool AddProductToWishList(int userId, int productId)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == productId);
            if (product == null)
                return false;

            if (_context.EShopUsers.Find(userId) == null)
                return false;


            var wish_list_product = _context.ProductToWishLists
                .SingleOrDefault(q => q.ProductId == productId &&
                    q.UserId == userId);

            if (wish_list_product == null)
            {
                wish_list_product = new ProductToWishList()
                {
                    ProductId = productId,
                    UserId = userId
                };
                _context.Add(wish_list_product);
                _context.SaveChanges();
            }
            return true;
        }


        public IQueryable<Product> GetUserWishList(int userId)
        {
            return _context.ProductToWishLists
                .Where(q => q.UserId == userId)
                .Select(q => q.Product);
        }

        public int GetWishListProductCount(int userId)
        {
            return _context.ProductToWishLists.Where(q => q.UserId == userId).Count();
        }

        public bool RemoveProductFromWishList(int userId, int productId)
        {
            var productOfWishList = _context.ProductToWishLists
                .SingleOrDefault(q => q.ProductId == productId
                && q.UserId == userId);
            if (productOfWishList == null)
                return false;
            _context.Remove(productOfWishList);
            _context.SaveChanges();
            return true;
        }
    }
}
