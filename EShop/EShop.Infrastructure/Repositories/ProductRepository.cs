using EShop.Domain.Entities;
using EShop.Domain.Interfaces;
using EShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly EShopDbContext _context;
		public ProductRepository(EShopDbContext context)
		{
			_context = context;
		}

		public void AddProduct(Product product)
		{
			_context.Add(product);
			_context.SaveChanges();
		}

		public Product? GetProduct(int productId)
		{
			return _context.Products.Find(productId);
		}

		public IQueryable<Category> GetProductCategories(int productId)
		{
			return _context.CategoryToProducts.Where(q => q.ProductId == productId)
				.Select(q => q.Category);
		}

		public IQueryable<Product> GetProducts()
		{
			return _context.Products;
		}

		public decimal GetProductTotalSale(int productId)
		{
			var orders = _context.Orders
				.Include(q => q.OrderDetails)
				.Where(q => q.IsFinaly).ToList();

			decimal totalSale = 0;
			orders.ForEach(q =>

				q.OrderDetails.ForEach(w =>
				{
					if (w.ProductId == productId)
						totalSale += w.Price;
				})
			);

			return totalSale;
		}

		public void RemoveProduct(Product product)
		{
			_context.Remove(product);
			_context.SaveChanges();
		}

		public void RemoveProductCategories(int productId, List<int> categories)
		{
			categories.ForEach(f =>
			{
				var temp = _context.CategoryToProducts
				.SingleOrDefault(q => q.CategoryId == f && q.ProductId == productId);
				if (temp != null)
					_context.Remove(temp);
			});
			_context.SaveChanges();
		}

		public void SetProductCategories(int productId, List<int> categories)
		{
			var all_categories = _context.Categories
				.Select(q => q.Id).ToList();
			var data = _context.CategoryToProducts
				.Where(q => q.ProductId == productId).ToList();
			categories.ForEach(q =>
			{
				if (!data.Any(c => c.CategoryId == q)
				&& all_categories.Contains(q))
					_context.Add(new CategoryToProduct()
					{
						CategoryId = q,
						ProductId = productId
					});
			});
			_context.SaveChanges();
		}

		public void UpdateProduct(Product product)
		{
			_context.Update(product);
			_context.SaveChanges();
		}
	}
}
