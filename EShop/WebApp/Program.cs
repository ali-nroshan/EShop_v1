using EShop.Core;
using EShop.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(config =>
{
    config.Filters.Add(new CheckUserRoleFilter());
});

builder.Services
    .AddInfrastructureDependency(builder.Configuration)
    .AddCoreDependency();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option =>
	{
		option.Cookie.Name = "EShopWebSite";
		option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
		option.LoginPath = "/Account";
		option.LogoutPath = "/Logout";
		option.ExpireTimeSpan = TimeSpan.FromDays(1);
		option.Cookie.HttpOnly= true;
	});

var app = builder.Build();


app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithRedirects("/404");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();
