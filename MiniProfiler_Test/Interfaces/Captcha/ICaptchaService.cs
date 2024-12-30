namespace MiniProfiler_Test.Interfaces.Captcha;

public interface ICaptchaService
{
    (string Code, byte[] Image) GenerateImageCaptcha();
}