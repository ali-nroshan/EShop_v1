using EShop.Core.Interfaces;
using EShop.Core.Services;
using EShop.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Componenets
{
    public class SearchBoxComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;
        public SearchBoxComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var data = categoryService.GetCategories()
                .Select(q => new CategoryViewModel()
                {
                    CategoryId = q.CategoryId,
                    CategoryName = q.CategoryName,
                }).ToList();

            return View("/Views/Components/SearchBoxComponentView.cshtml", data);
        }

    }
}
