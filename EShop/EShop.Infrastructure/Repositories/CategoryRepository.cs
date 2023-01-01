using EShop.Domain.Entities;
using EShop.Domain.Interfaces;
using EShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EShopDbContext _context;

        public CategoryRepository(EShopDbContext context)
        {
            _context = context;
        }

		public void AddCategory(string categoryName, string categoryLogoName)
		{
            _context.Categories.Add(new Category()
            {
                Name = categoryName,
                CategoryLogoName = categoryLogoName
            });
            _context.SaveChanges();
		}

        public void RemoveCategory(Category category)
        {
            _context.Remove(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();
        }

		public IQueryable<Category> GetCategories()
        {
            return _context.Categories;
        }

        public Category? GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public IQueryable<Product> GetCategoryProducts(int categoryId)
        {
            return _context.CategoryToProducts
                .Where(q => q.CategoryId == categoryId)
                .Select(q => q.Product);
        }

        public int GetCategoryProductsCount(int categoryId)
        {
            return _context.CategoryToProducts
                .Where(q=>q.CategoryId== categoryId)
                .Count();
        }

        public decimal GetCategoryTotalSale(int categoryId)
        {
            decimal totalSale = 0;
            var sub_products = _context.CategoryToProducts
                .Where(q => q.CategoryId == categoryId).Select(q => q.Product)
                .ToList();
            var successful_orders
                = _context.Orders
                .Include(o => o.OrderDetails).Where(q => q.IsFinaly).ToList();

            successful_orders
                .ForEach(q => q.OrderDetails.ForEach(o =>
                {
                    if (sub_products.Any(p => p.ProductId == o.ProductId))
                        totalSale += o.Price;
                }));

            return totalSale;
        }
    }
}
