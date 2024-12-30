using System.ComponentModel.DataAnnotations;

namespace MiniProfiler_Test.Models.Api.Users;

public class LoginApiVm
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MinLength(3, ErrorMessage = "{0} نمیتواند کمتر از {1} کاراکتر باشد.")]
    [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "مرا به خاطر بسپار")] public bool RememberMe { get; set; }

    [Display(Name = "کد امنیتی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MinLength(6, ErrorMessage = "{0} نمیتواند کمتر از {1} کاراکتر باشد.")]
    [MaxLength(6, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
    public string CaptchaInput { get; set; } = string.Empty;
}