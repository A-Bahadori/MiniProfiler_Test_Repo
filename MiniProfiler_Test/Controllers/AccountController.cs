using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using MiniProfiler_Test.Extensions;
using MiniProfiler_Test.Interfaces.Api;
using MiniProfiler_Test.Models.Api.Users;

namespace MiniProfiler_Test.Controllers;

public class AccountController : SiteBaseController
{
    #region Constractor

    private readonly IApiAuthenticateService _authenticateService;
    private readonly IApiUserService _userService;

    public AccountController(IApiAuthenticateService authenticateService, IApiUserService userService)
    {
        _authenticateService = authenticateService;
        _userService = userService;
    }

    #endregion

    #region Login

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginApiVm model, string? returnUrl = null)
    {
        #region Model State

        ModelState.Remove("returnUrl");
        if (!ModelState.IsValid) return View(model);

        #endregion

        #region Sanitizer

        var sanitizer = new HtmlSanitizer();
        model.Username = sanitizer.Sanitize(model.Username);
        model.Password = sanitizer.Sanitize(model.Password);
        model.CaptchaInput = sanitizer.Sanitize(model.CaptchaInput);

        #endregion

        #region Captcha

        // var captchaCode = HttpContext.Session.GetString("ImageCaptchaCode");
        //
        // if (captchaCode == null || !string.Equals(captchaCode, model.CaptchaInput, StringComparison.CurrentCultureIgnoreCase))
        // {
        //     ModelState.AddModelError("CaptchaInput", "کد امنیتی نادرست است.");
        //     return View(model);
        // }

        var isValidCaptcha = HttpContext.Session.ValidateImageCaptcha(model.CaptchaInput);

        if (!isValidCaptcha)
        {
            ModelState.AddModelError("", "کد امنیتی صحیح نیست");
            ModelState.Remove("CaptchaInput");
            model.CaptchaInput = string.Empty;
            return View(model);
        }

        #endregion
        
        try
        {
            var loginResult = await _authenticateService.LoginApiAsync(model);
            if (loginResult.Success)
            {
                TempData[SuccessMessage] = $"{loginResult.Data.FirstName} {loginResult.Data.LastName} عزیز، خوش آمدید.";

                var userRole= loginResult.Data.Role;

                if (userRole.Contains("Admin") && returnUrl == null)
                {
                    returnUrl = "~/Admin";
                }
                else if (userRole.Contains("User") && returnUrl == null)
                {
                    returnUrl = "~/User";
                }
                else
                {
                    returnUrl ??= Url.Content("~/");
                }

                return LocalRedirect(returnUrl);
            }

            if (loginResult.StatusCode == 401)
            {
                TempData[ErrorMessage] = "نام کاربری یا رمز عبور اشتباه است";
            }
            else
            {
                TempData[ErrorMessage] = loginResult.Message;
            }

            return View(model);
        }
        catch
        {
            TempData[ErrorMessage] = "خطا در اتصال";
            return View(model);
        }
    }

    #endregion
    
    #region Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _authenticateService.LogoutAsync();
        TempData[SuccessMessage] = "خروج با موفقیت انجام شد";
        return LocalRedirect("/Account/Login");
    }
    #endregion
}