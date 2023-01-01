using EShop.Core.Interfaces;
using EShop.Core.Services;
using EShop.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Componenets
{
    public class ShowCategoriesWithLogoComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;
        public ShowCategoriesWithLogoComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = categoryService.GetCategoryWithLogos()
                .Select(q => new CategoryWithLogoViewModel()
                {
                    CategoryId = q.CategoryId,
                    CategoryLogoName = q.CategoryLogoName,
                    CategoryName = q.CategoryName,
                    SubProductsCount = categoryService.GetCategoryProductsCount(q.CategoryId)
                }).ToList();
            return View("/Views/Components/ShowCategoriesWithLogoViewComponent.cshtml",
                categories);
        }
    }
}
