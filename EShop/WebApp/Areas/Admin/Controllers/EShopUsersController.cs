using EShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.Models;
using WebApp.Controllers;
using WebApp.Filters;

namespace WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class EShopUsersController : CustomBaseController
	{
		private readonly IUserService userService;

		public EShopUsersController(IUserService userService)
		{
			this.userService = userService;
		}

		public IActionResult Index()
		{
			var data = userService.GetUserReports();
			return View(data);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create([Bind("Email,Password,IsAdmin")] EShopUserCreateOrEditViewModel eShopUser)
		{
			if (!ModelState.IsValid)
				return View(eShopUser);

			if (userService.EmailExist(eShopUser.Email))
			{
				ModelState.AddModelError("Email", $"{eShopUser.Email} has been already registered");
				return View(eShopUser);
			}

			userService.AddUser(eShopUser.Email
				, eShopUser.Password, eShopUser.IsAdmin);

			return LocalRedirect("/Admin/EShopUsers");
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var eShopUser = userService.GetUserById(id.Value);
			if (eShopUser == null)
			{
				return NotFound();
			}
			return View(new EShopUserCreateOrEditViewModel()
			{
				Email = eShopUser.UserEmail,
				IsAdmin = eShopUser.UserRole=="Admin",
				Password = eShopUser.Password,
				UserId = eShopUser.UserId
			});
		}

		[HttpPost]
		public IActionResult Edit([Bind("UserId,Email,Password,IsAdmin")] EShopUserCreateOrEditViewModel eShopUser)
		{
			if (!ModelState.IsValid)
				return View(eShopUser);

			try
			{
				userService.UpdateUserCredentials(eShopUser.UserId
					,eShopUser.Password,eShopUser.Email,eShopUser.IsAdmin);
			}
			catch
			{
				ModelState.AddModelError("Email", "Something is wrong...");
				return View(eShopUser);
			}
			return LocalRedirect("/Admin/EShopUsers");
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var eShopUser = userService.GetUserById(id.Value);
			if (eShopUser == null)
			{
				return NotFound();
			}

			return View(eShopUser);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(int id)
		{
			try
			{
				userService.DeleteUser(id);
			}
			catch
			{
				return NotFound();
			}
			UsersListCheckerTool.AddUserToRemoveList(id);

			return LocalRedirect("/Admin/EShopUsers");
		}
	}
}
