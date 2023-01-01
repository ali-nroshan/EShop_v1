using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp.Controllers
{
    public class CartController : CustomBaseController
    {
        private readonly IOrderService orderService;
        private int _userId
        {
            get
            {
                return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString());
            }
        }
        public CartController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Authorize, Route("AddToCart")]
        public IActionResult AddToCart(int productId, int count)
        {
            if (count <= 0)
                return NotFound();

            bool rs = orderService.AddProductToOrder(_userId, productId, count);
            if (!rs)
                return NotFound();

            return RedirectToAction("ShowCart");
        }

        [Authorize, Route("RemoveProductFromCart")]
        public IActionResult RemoveProductFromCart(int productId)
        {
            if (!orderService.RemoveProductFromOrder(_userId, productId))
                return NotFound();

            return RedirectToAction("ShowCart");
        }

        [HttpGet("ShowCart")]
        public IActionResult ShowCart()
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("LoginOrSignUp", "Account");

            var order = orderService.GetUserOrder(_userId);
            if (order == null)
                return View();

            return View(order);
        }

        [Authorize, HttpPost("UpdateCart")]
        public IActionResult UpdateCart(List<UpdateCartViewModel> updateCarts)
        {
            if (!ModelState.IsValid || updateCarts.Count > 50)
                return NotFound();
            foreach (var item in updateCarts)
                _ = orderService.AddProductToOrder(_userId, item.ProductId, item.Count);
            return RedirectToAction("ShowCart");
        }

        [Authorize, Route("PayCart")]
        public IActionResult FinalizeCart()
        {
            bool rs = orderService.FinalizeOrder(_userId);
            if (!rs)
                return NotFound();

            return View("SuccessfulPayment", User.FindFirstValue(ClaimTypes.Name));
        }
    }
}
