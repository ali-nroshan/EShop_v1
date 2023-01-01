using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class CustomBaseController : Controller
	{
		public new IActionResult NotFound()
		{
			return View("~/Views/Errors/NotFoundPage.cshtml");
		}
	}
}
