using System.ComponentModel.DataAnnotations;

namespace MiniProfiler_Test.Models.Api.Users;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "نام کاربری الزامی است")]
    [Display(Name = "نام کاربری")]
    [MinLength(4, ErrorMessage = "نام کاربری باید حداقل 4 کاراکتر باشد")]
    [MaxLength(200, ErrorMessage = "نام کاربری نمی‌تواند بیشتر از 200 کاراکتر باشد")]
    public string Username { get; set; }

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [Display(Name = "رمز عبور")]
    [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
    [MaxLength(200, ErrorMessage = "رمز عبور نمی‌تواند بیشتر از 200 کاراکتر باشد")]
    public string Password { get; set; }

    [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
    [Display(Name = "تکرار رمز عبور")]
    [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن باید یکسان باشند")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "نام الزامی است")]
    [Display(Name = "نام")]
    [MaxLength(200, ErrorMessage = "نام نمی‌تواند بیشتر از 200 کاراکتر باشد")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "نام خانوادگی الزامی است")]
    [Display(Name = "نام خانوادگی")]
    [MaxLength(200, ErrorMessage = "نام خانوادگی نمی‌تواند بیشتر از 200 کاراکتر باشد")]
    public string LastName { get; set; }

    [Display(Name = "نقش")]
    [Required(ErrorMessage = "انتخاب نقش الزامی است")]
    public string Role { get; set; }
}
