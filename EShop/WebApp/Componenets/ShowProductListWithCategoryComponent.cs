using EShop.Core.Interfaces;
using EShop.Core.Services;
using EShop.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Componenets
{
    public class ShowProductListWithCategory : ViewComponent
    {
        private readonly IProductOfferService productOfferService;
        public ShowProductListWithCategory(IProductOfferService productOfferService)
        {
            this.productOfferService = productOfferService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CategoryBaseOfferOption option)
        {
            var data = productOfferService.GetCategoryBaseOffers(option);
            await Task.CompletedTask;
            return View("/Views/Components/ShowProductListWithCategoriesComponentView.cshtml", data);
        }

    }
}
