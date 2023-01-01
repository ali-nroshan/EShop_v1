using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Componenets
{
    public class MiniCartComponent : ViewComponent
    {
        private readonly IOrderService orderService;
        public MiniCartComponent(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IViewComponentResult Invoke(int userId = 0)
        {
            var order = orderService.GetUserOrder(userId);
            var miniCartData = new MiniCartViewModel();
            if (order != null)
            {
                miniCartData.TotalPrice = order.TotalPrice;
                miniCartData.MiniCartItems.AddRange(order.CartItems
                    .Select(q => new MiniCartItemViewModel()
                    {
                        ProductCount = q.ProductCountForCart,
                        ProductImage = q.ProductImage,
                        ProductId = q.ProductId,
                        ProductName = q.ProductName,
                        TotoalPrice = q.PriceSum,
                        ProductPrice = q.ProductPrice
                    }).ToList());
            }
            else
                miniCartData.TotalPrice = 0;

            return View("/Views/Components/MiniCartComponentView.cshtml", miniCartData);
        }
    }
}
