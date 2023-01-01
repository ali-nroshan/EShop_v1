using EShop.Core.Interfaces;
using EShop.Core.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.Models;
using WebApp.Controllers;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : CustomBaseController
	{
		private readonly ICategoryService categoryService;
		

		public CategoryController(ICategoryService categoryService)
		{
			this.categoryService = categoryService;
		}

		public IActionResult Index()
		{
			var categories_report = categoryService.GetCategoryWithLogos()
				.Select(q => new CategorySummaryViewModel()
				{
					CategoryId = q.CategoryId,
					CategoryName = q.CategoryName,
					CategoryLogo = q.CategoryLogoName,
					SubProductsCount = categoryService.GetCategoryProductsCount(q.CategoryId),
					CategoryTotalSale = categoryService.GetCategoryTotalSale(q.CategoryId)
				}).ToList();
			return View(categories_report);
		}

		// GET: Admin/Categories/Details/5
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = categoryService.GetCategoryById(id.Value);
			if (category == null)
			{
				return NotFound();
			}

			var data = new CategoryReportViewModel()
			{
				CategoryId = category.CategoryId,
				CategoryName = category.CategoryName
			};

			data.Products.AddRange(categoryService.GetCategoryProducts(category.CategoryId)
				.Select(q => new ProductSummaryViewModel()
				{
					ProductId = q.ProductId,
					ProductImageFile = q.ProductImageFileName,
					ProductName = q.ProductName,
					ProductScore = q.ProductScore,
					InventoryState = (q.QuantityInStock > 0)
				}).ToList());

			return View(data);
		}

		// GET: Admin/Categories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Admin/Categories/Create
		[HttpPost]
		public IActionResult Create([Bind("CategoryName,CategoryLogo")] CategoryViewModel category)
		{
			if (!ModelState.IsValid)
				return View(category);

			if (category.CategoryLogo == null || category.CategoryLogo.Length <= 0
				|| !IStorageService.CheckFileSize(category.CategoryLogo.Length))
			{
				ModelState.AddModelError("CategoryName", "Something is wrong...");
				return View(category);
			}

			byte[] image = new byte[category.CategoryLogo.Length];

			var stream = category.CategoryLogo.OpenReadStream();
			stream.Read(image);

			stream.Close();

			categoryService.AddCategory(category.CategoryName, image,
				Path.GetExtension(category.CategoryLogo.FileName));
			return LocalRedirect("/Admin/Category");
		}

		// GET: Admin/Categories/Edit/5
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = categoryService.GetCategoryById(id.Value);
			if (category == null)
			{
				return NotFound();
			}
			return View(new CategoryViewModel()
			{
				CategoryId = category.CategoryId,
				CategoryName = category.CategoryName
			});
		}

		// POST: Admin/Categories/Edit/5
		[HttpPost]
		public IActionResult Edit([Bind("CategoryId,CategoryName,CategoryLogo")] CategoryViewModel category)
		{
			if (!ModelState.IsValid)
				return View(category);

			if (category.CategoryLogo!=null)
				if (!IStorageService.CheckFileSize(category.CategoryLogo.Length)
					|| category.CategoryLogo.Length <= 0)
				{
					ModelState.AddModelError("CategoryName", "Something is wrong...");
					return View(category);
				}

			var r_category = categoryService.GetCategoryById(category.CategoryId);
			if (r_category == null)
				return NotFound();

			if (category.CategoryLogo != null)
			{
				var image = new byte[category.CategoryLogo.Length];

				using (var stream = category.CategoryLogo.OpenReadStream())
				{
					stream.Read(image);
				}

				categoryService.UpdateCategoryLogo(category.CategoryId,
					image,
					Path.GetExtension(category.CategoryLogo.FileName));
			}
			if (r_category.CategoryName != category.CategoryName)
				categoryService.UpdateCategoryName(category.CategoryId,category.CategoryName);
			return LocalRedirect("/Admin/Category");
		}

		// GET: Admin/Categories/Delete/5
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = categoryService.GetCategoryWithLogoById(id.Value);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// POST: Admin/Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(int id)
		{
			var category = categoryService.GetCategoryById(id);
			if (category == null)
				return NotFound();

			categoryService.RemoveCategory(id);
			return LocalRedirect("/Admin/Category");
		}

	}
}
