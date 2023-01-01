using System.ComponentModel.DataAnnotations;

namespace EShop.Core.ViewModels
{

    public class AuthenticationViewModel
    {
        public RegisterViewModel? RegisterViewModel { get; set; }

        public LoginViewModel? LoginViewModel { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید "), EmailAddress(ErrorMessage = "فرمت ایمیل ورودی اشتباه میباشد"), MaxLength(100, ErrorMessage = "{0} از 100 تا حرف بیشتر نمیتواند باشد")]
        public string Email { get; set; } = null!;

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید "), DataType(DataType.Password), MaxLength(50, ErrorMessage = "{0} از 100 تا حرف بیشتر نمیتواند باشد")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید "), EmailAddress(ErrorMessage = "فرمت ایمیل ورودی اشتباه میباشد"), MaxLength(100, ErrorMessage = "{0} از 100 تا حرف بیشتر نمیتواند باشد")]
        public string Email { get; set; } = null!;

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید "), DataType(DataType.Password), MaxLength(50, ErrorMessage = "{0} از 50 تا حرف بیشتر نمیتواند باشد")]
        public string Password { get; set; } = null!;

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید "), Compare("Password", ErrorMessage = "تکرار رمز عبور با رمز عبور تطابق ندارد"), DataType(DataType.Password), MaxLength(50, ErrorMessage = "{0} از 100 تا حرف بیشتر نمیتواند باشد")]
        public string? RePassword { get; set; }
    }

}
