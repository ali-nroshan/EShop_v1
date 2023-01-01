using EShop.Core.Interfaces;
using EShop.Domain.Interfaces;
using EShop.Infrastructure.Common;
using EShop.Infrastructure.Context;
using EShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure
{
	public static class DependencyContainer
	{
		public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services,
		   IConfiguration configuration)
		{
			_ = services.AddDbContext<EShopDbContext>(option =>
			{
				option.UseSqlServer(configuration.GetConnectionString("EShopDbContext"));
			});

			_ = services.AddScoped<IProductRepository, ProductRepository>()
				.AddScoped<ICategoryRepository, CategoryRepository>()
				.AddScoped<IOrderRepository, OrderRepository>()
				.AddScoped<IUserRepository, UserRepository>()
				.AddScoped<IUserWishListRepository, UserWishListRepository>()
				.AddSingleton<ICryptographicService, CryptographicService>()
				.AddSingleton<IStorageService, StorageService>();

			return services;
		}
	}
}
