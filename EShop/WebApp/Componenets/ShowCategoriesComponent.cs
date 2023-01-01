using EShop.Core.Interfaces;
using EShop.Core.Services;
using EShop.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Componenets
{
    public class ShowCategoriesComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;
        public ShowCategoriesComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = categoryService.GetCategories()
                .Select(q=>new CategoryViewModel()
                {
                    CategoryId = q.CategoryId,
                    CategoryName = q.CategoryName
                }).ToList();
            return View("/Views/Components/ShowCategoriesComponentView.cshtml", categories);
        }
    }
}
