using EShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.Areas.Admin.Models;
using EShop.Core.Services;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : CustomBaseController
	{
		private readonly IProductService productService;
		private readonly ICategoryService categoryService;

		public ProductController(IProductService productService,
						   ICategoryService categoryService)
		{
			this.productService = productService;
			this.categoryService = categoryService;
		}

		public IActionResult Index()
		{
			var products = productService.GetProducts();

			foreach (var product in products)
			{
				ViewData[$"TotalSale{product.ProductId}"] = productService.GetProductTotalSale(product.ProductId);
			}

			return View(products);
		}

		// GET: Admin/Product/Details/5
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = productService.GetProduct(id.Value);
			if (product == null)
			{
				return NotFound();
			}

			ViewBag.ProductCategories = productService.GetProductCategories(id.Value);

			return View(product);
		}

		// GET: Admin/Product/Create
		public IActionResult Create()
		{
			FillCategoriesInViewBag();
			return View();
		}

		private void FillCategoriesInViewBag()
		{
			ViewBag.Categories = categoryService.GetCategories().Select(q => new CategoryViewModel()
			{
				CategoryId = q.CategoryId,
				CategoryName = q.CategoryName
			}).ToList();
		}


		// POST: Admin/Product/Create
		[HttpPost]
		public IActionResult Create([Bind("ProductName,ProductImage,ProductDescription,QuantityInStock,ProductPrice,ProductScore,SelectedCategories")] ProductViewModel product)
		{
			if (!ModelState.IsValid || product.ProductImage == null || product.ProductImage?.Length <= 0)
			{
				FillCategoriesInViewBag();

				ModelState.AddModelError("ProductName", "Something is wrong...");
				return View(product);
			}
			if (!IStorageService.CheckFileSize(product.ProductImage!.Length))
			{
				ModelState.AddModelError("ProductImage", "File is too large...");
				return View(product);
			}

			var rproduct = new EShop.Core.ViewModels.ProductViewModel()
			{
				ProductName = product.ProductName,
				ProductPrice = product.ProductPrice,
				ProductDescription = product.ProductDescription,
				ProductScore = product.ProductScore,
				QuantityInStock = product.QuantityInStock
			};

			var productImage = new byte[product.ProductImage.Length];
			using (var stream = product.ProductImage.OpenReadStream())
			{
				stream.Read(productImage);
			}

			productService.AddProduct(rproduct, productImage,
				Path.GetExtension(product.ProductImage.FileName));


			product.SelectedCategories = product.SelectedCategories.Distinct().ToList();
			productService.SetProductCategories(rproduct.ProductId, product.SelectedCategories);

			return LocalRedirect("/Admin/Product");
		}

		// GET: Admin/Product/Edit/5
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = productService.GetProduct(id.Value);
			if (product == null)
			{
				return NotFound();
			}


			var vproduct = new ProductViewModel()
			{
				ProductDescription = product.ProductDescription,
				ProductId = product.ProductId,
				ProductName = product.ProductName,
				ProductPrice = product.ProductPrice,
				ProductScore = product.ProductScore,
				QuantityInStock = product.QuantityInStock
			};

			FillCategoriesInViewBag();
			vproduct.SelectedCategories = productService.GetProductCategories(id.Value)
				.Select(q => q.CategoryId).ToList();

			return View(vproduct);
		}


		// POST: Admin/Product/Edit/5
		[HttpPost]
		public IActionResult Edit([Bind("ProductId,ProductName,ProductImage,ProductDescription,QuantityInStock,ProductPrice,ProductScore,SelectedCategories")] ProductViewModel product)
		{
			if (product == null)
				return NotFound();

			if (!ModelState.IsValid || product.ProductImage?.Length <= 0)
			{
				FillCategoriesInViewBag();
				product.SelectedCategories = productService.GetProductCategories(product.ProductId)
					.Select(q => q.CategoryId).ToList();
				return View(product);
			}

			if (product.ProductImage != null)
				if (!IStorageService.CheckFileSize(product.ProductImage!.Length))
				{
					FillCategoriesInViewBag();
					ModelState.AddModelError("ProductImage", "File is too large...");
					return View(product);
				}

			//up here
			var rproduct = productService.GetProduct(product.ProductId);
			if (rproduct == null)
				return NotFound();


			var categories_id = categoryService.GetCategories()
				.Select(q => q.CategoryId).ToList();

			product.SelectedCategories = product.SelectedCategories.Distinct()
				.Where(q => categories_id.Contains(q)).ToList();

			if (product.SelectedCategories.Count == 0)
			{
				FillCategoriesInViewBag();
				ModelState.AddModelError("ProductName", "Categories is null");
				return View(product);
			}

			var product_categories = productService.GetProductCategories(rproduct.ProductId)
				.Select(q => q.CategoryId).ToList();

			var add_list = product.SelectedCategories
				.Where(q => !product_categories.Contains(q)).ToList();

			var delete_list = product_categories.Where(q => !product.SelectedCategories.Contains(q)).ToList();

			if (add_list.Count != 0)
				productService.SetProductCategories(product.ProductId, add_list);

			if (delete_list.Count != 0)
				productService.RemoveProductCategories(product.ProductId, delete_list);

			rproduct.QuantityInStock = product.QuantityInStock;
			rproduct.ProductScore = product.ProductScore;
			rproduct.ProductName = product.ProductName;
			rproduct.ProductPrice = product.ProductPrice;
			rproduct.ProductDescription = product.ProductDescription;

			if (product.ProductImage?.Length > 0)
			{
				var productImage = new byte[product.ProductImage.Length];
				using (Stream stream = product.ProductImage.OpenReadStream())
				{
					stream.Read(productImage);
				}

				productService.UpdateProductImage(product.ProductId, productImage,
					Path.GetExtension(product.ProductImage.FileName));
			}

			productService.UpdateProduct(rproduct);
			return LocalRedirect("/Admin/Product");
		}

		// GET: Admin/Product/Delete/5
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = productService.GetProduct(id.Value);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Admin/Product/Delete/5
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(int id)
		{
			var product = productService.GetProduct(id);
			if (product == null)
				return NotFound();


			productService.RemoveProduct(id);
			return LocalRedirect("/Admin/Product");
		}

	}
}
