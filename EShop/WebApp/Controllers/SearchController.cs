using EShop.Core.Interfaces;
using EShop.Core.ViewModels;
using EShop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class SearchController : CustomBaseController
    {
        private readonly ISearchService searchService;

        private ProductItemViewModel ProductToProductItemVM(Product product)
        {
            return new ProductItemViewModel()
            {
                HasInventory = product.QuantityInStock > 0 ? true : false,
                ProductDescription = product.ProductDescription,
                ProductId = product.ProductId,
                ProductImageFile = product.ProductImageFileName,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductScore = product.ProductScore ?? 0
            };
        }


        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [Route("Category/{categoryId}/{categoryName}")]
        public IActionResult SearchCategory(int categoryId)
        {
            string categoryName = string.Empty;
            var products = searchService.GetCategoryProducts(categoryId, ref categoryName);
            if (products.Count == 0)
                return NotFound();

            var vproducts = products
                .Select(q => ProductToProductItemVM(q)).ToList();
            ViewBag.ShowProductsViewTitle = $"نمایش محصولات گروه {categoryName}";
            return View("/Views/Product/ShowProducts.cshtml", vproducts);
        }


        [HttpPost("Search")]
        public IActionResult SearchProducts(string searchTerm, int? categoryId)
        {
            if (!ModelState.IsValid || searchTerm.Length > 50)
                return NotFound();

            var products = searchService.SearchProducts(searchTerm.ToLower(), categoryId);
            if (products.Count == 0)
                return NotFound();

            var view_argument = products
                .Select(q => ProductToProductItemVM(q)).ToList();

            ViewBag.ShowProductsViewTitle = "جستجوی محصولات";
            return View("/Views/Product/ShowProducts.cshtml", view_argument);
        }
    }
}
