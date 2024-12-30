using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MiniProfiler_Test.Interfaces.Captcha;

namespace MiniProfiler_Test.Controllers;

public class CaptchaController : SiteBaseController
{
    #region Constractors

    private readonly ICaptchaService _captchaService;

    public CaptchaController(ICaptchaService captchaService)
    {
        _captchaService = captchaService;
    }

    #endregion

    [HttpGet]
    [EnableRateLimiting("captchaPolicy")]
    public IActionResult GenerateImageCaptcha()
    {
        var (captchaCode, captchaImage) = _captchaService.GenerateImageCaptcha();
        HttpContext.Session.SetString("ImageCaptchaCode", captchaCode);
        return File(captchaImage, "image/png");
        return NoContent();
    }
}