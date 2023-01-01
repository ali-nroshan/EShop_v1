using EShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProductController : CustomBaseController
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [Route("Product/{productId}")]
        public IActionResult ProductDetail(int productId)
        {
            var product = productService.GetProduct(productId);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
