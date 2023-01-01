using EShop.Core.Interfaces;
using EShop.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core
{
	public static class DependencyContainer
	{
		public static IServiceCollection AddCoreDependency(this IServiceCollection services)
		{
			_ = services.AddScoped<IOrderService, OrderService>()
				.AddScoped<IProductOfferService, ProductOfferService>()
				.AddScoped<ISearchService, SearchService>()
				.AddScoped<ICategoryService, CategoryService>()
				.AddScoped<IUserService, UserService>()
				.AddScoped<IProductService, ProductService>();

			return services;
		}
	}
}
