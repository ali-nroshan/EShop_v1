using EShop.Domain.Entities;
using EShop.Domain.Interfaces;
using EShop.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly EShopDbContext _context;

		public UserRepository(EShopDbContext context)
		{
			_context = context;
		}

		public void AddUser(EShopUser user)
		{
			_context.Add(user);
			_context.SaveChanges();
		}


		public IQueryable<EShopUser> GetEShopUsers()
		{
			return _context.EShopUsers;
		}

		public EShopUser? GetUser(string email, string password)
		{
			return _context.EShopUsers.SingleOrDefault(u => u.Email == email
			&& u.Password == password);
		}

		public EShopUser? GetUserByEmail(string email)
		{
			return _context.EShopUsers.SingleOrDefault(q => q.Email == email);
		}

		public EShopUser? GetUserById(int userId)
		{
			return _context.EShopUsers.Find(userId);
		}

		public void GetUserSuccessfulOrderCountAndTotalPrice(int userId, ref int successfulOrdersCount, ref decimal totalPrice)
		{
			var user = _context.EShopUsers.Find(userId);
			if (user == null)
				throw new Exception("User not exist 404");
			decimal temp = 0;
			var successfulOrders = _context.Orders
				.Include(o => o.OrderDetails)
				.Where(q => q.UserId == userId && q.IsFinaly).ToList();
			successfulOrdersCount = successfulOrders.Count();
			successfulOrders.ForEach(o =>
			{
				o.OrderDetails.ForEach(od =>
				{
					temp += od.Price;
				});
			});
			totalPrice = temp;
		}

		public void RemoveUser(EShopUser user)
		{
			_context.Remove(user);
			_context.SaveChanges();
		}

		public void UpdateUser(EShopUser user)
		{
			_context.Update(user);
			_context.SaveChanges();
		}
	}
}
