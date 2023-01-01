using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("/Error")]
        public IActionResult Index()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception is not null)
            {
                return exception.Message.TakeLast(3).ToString() switch
                {
                    "404" => NotFoundError(),
                    "401" => NotFoundError(),
                    "500" => NotFoundError(),
					_ => NotFoundError()
                };
            }
            return Redirect("/");
        }

        [Route("/404")]
        public ViewResult NotFoundError()
        {
            return View("NotFoundPage");
        }
    }
}
