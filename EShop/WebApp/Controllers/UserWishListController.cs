using EShop.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserWishListController : CustomBaseController
    {
        private readonly IUserService userService;

        public UserWishListController(IUserService userService)
        {
            this.userService = userService;
        }

        private int _userId
        {
            get
            {
                return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString());
            }
        }


        [Route("WishList")]
        public IActionResult GetUserWishList()
        {
            var products = userService.GetUserWishList(_userId);
            if (products == null)
                return NotFound();
            return View("UserWishList", products);
        }

        [Route("AddToWishList/{productId}")]
        public IActionResult AddProductToWishList(int productId)
        {
            if (!userService.AddProductToWishList(_userId, productId))
                return NotFound();
            return RedirectToAction("GetUserWishList");
        }

        [Route("RemoveFromWishList/{productId}")]
        public IActionResult RemoveProductFromWishList(int productId)
        {
            if (!userService.RemoveProductFromWishList(_userId, productId))
                return NotFound();
            return RedirectToAction("GetUserWishList");
        }
    }
}
