using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Componenets
{
    public class ShowProductListComponent : ViewComponent
    {
        private readonly IProductOfferService productOfferService;
        public ShowProductListComponent(IProductOfferService productOfferService)
        {
            this.productOfferService = productOfferService;
        }

        public IViewComponentResult Invoke(int productId, ProductBaseOfferOption option)
        {
            var data = productOfferService.GetProductBaseOffers(productId, option);
            return View("/Views/Components/ShowProductListComponentView.cshtml", data);
        }
    }
}
