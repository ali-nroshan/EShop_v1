using EShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Componenets
{
    public class WishListComponent : ViewComponent
    {
        private readonly IUserService userService;
        public WishListComponent(IUserService userService)
        {
            this.userService = userService;
        }

        public IViewComponentResult Invoke(int userId = -1)
        {
            int count = (userId == -1) ? 0 : userService
                .GetWishListProductCount(userId);
            return View("/Views/Components/WishListViewComponent.cshtml", count);
        }
    }
}
