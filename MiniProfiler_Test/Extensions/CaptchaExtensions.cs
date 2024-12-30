namespace MiniProfiler_Test.Extensions;

using Microsoft.AspNetCore.Http;

public static class CaptchaExtensions
{
    public static bool ValidateImageCaptcha(this ISession session, string userAnswer)
    {
        if (string.IsNullOrEmpty(userAnswer))
            return false;

        var correctAnswer = session.GetString("ImageCaptchaCode");
        if (string.IsNullOrEmpty(correctAnswer))
            return false;

        session.Remove("ImageCaptchaCode");

        return string.Equals(correctAnswer.Trim(), userAnswer.Trim(), StringComparison.CurrentCultureIgnoreCase);
    }
}