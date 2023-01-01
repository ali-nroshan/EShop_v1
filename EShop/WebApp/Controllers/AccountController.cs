using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EShop.Core.Interfaces;
using EShop.Core.ViewModels;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("Account")]
        public IActionResult LoginOrSignUp()
        {
            if (User.Identity!.IsAuthenticated)
                return Redirect("/");

            return View();
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([Bind("RegisterViewModel")] AuthenticationViewModel register)
        {
            if (User.Identity!.IsAuthenticated)
                return Redirect("/");

            if (!ModelState.IsValid || register.RegisterViewModel == null
                || register.RegisterViewModel.Password.Length < 8)
                return View("LoginOrSignUp", register);

            if (_userService.EmailExist(register.RegisterViewModel.Email))
            {
                ModelState.AddModelError("RegisterViewModel.Email", $"ایمیل {register.RegisterViewModel.Email} قبلا ثبت شده است");
                return View("LoginOrSignUp", register);
            }

            _userService.AddUser(register.RegisterViewModel.Email,
                register.RegisterViewModel.Password,
                false);

            return View("SuccessRegister", register.RegisterViewModel.Email);
        }

        [HttpPost("Login")]
        public IActionResult Login([Bind("LoginViewModel")] AuthenticationViewModel login)
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid || login.LoginViewModel == null
                || login.LoginViewModel.Password.Length < 8)
            {
                return View("LoginOrSignUp", login);
            }

            var user = _userService.GetUser(login.LoginViewModel.Email,
                login.LoginViewModel.Password);

            if (user == null)
            {
                ModelState.AddModelError("LoginViewModel.Email", "کاربری یافت نشد !");
                return View("LoginOrSignUp", login);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("IsAdmin", user.IsAdmin.ToString()),
            };
            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.LoginViewModel.RememberMe
            };

            _ = HttpContext.SignInAsync(principal, properties);

            return RedirectToAction("Index", "Home");
        }

        [Authorize, Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
