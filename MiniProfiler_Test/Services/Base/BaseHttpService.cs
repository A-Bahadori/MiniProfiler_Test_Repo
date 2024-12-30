using System.Net.Http.Headers;
using MiniProfiler_Test.Interfaces.Storage;
using System.Text.Json;

namespace MiniProfiler_Test.Services.Base;

public class BaseHttpService
{
    protected readonly IClient _client;
    protected readonly ILocalStorageService _localStorageService;

    public BaseHttpService(IClient client, ILocalStorageService localStorageService)
    {
        _client = client;
        _localStorageService = localStorageService;
    }

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
    {
        var apiResponse = new Response<Guid> { Success = false, StatusCode = exception.StatusCode };

        try
        {
            // تلاش برای تبدیل پاسخ خطا به مدل مورد نظر
            if (exception.Response != null)
            {
                var errorContent = JsonSerializer.Deserialize<ApiErrorResponse>(
                    exception.Response.ToString(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (errorContent != null)
                {
                    apiResponse.Message = errorContent.Message;
                    apiResponse.ValidationErrors = errorContent.Errors;
                    return apiResponse;
                }
            }
        }
        catch (JsonException jsonEx)
        {
            // بررسی نوع خطا بر اساس StatusCode
            apiResponse.Message = exception.StatusCode switch
            {
                400 => "داده‌های ارسالی نامعتبر است. لطفاً ورودی‌های خود را بررسی کنید.",
                404 => "اطلاعات درخواستی یافت نشد.",
                _ => "خطا در پردازش پاسخ سرور. لطفاً دوباره تلاش کنید."
            };

            // اضافه کردن جزئیات خطا
            apiResponse.ValidationErrors = new List<ValidationError>
    {
        new ValidationError
        {
            PropertyName = "ResponseParsing",
            ErrorMessage = $"خطا در پردازش پاسخ: {jsonEx.Message}"
        }
    };
        }
        catch (Exception ex)
        {
            apiResponse.Message = "خطای غیرمنتظره در پردازش پاسخ سرور";
            apiResponse.ValidationErrors = new List<ValidationError>
    {
        new ValidationError
        {
            PropertyName = "UnexpectedError",
            ErrorMessage = "خطای غیرمنتظره در پردازش پاسخ"
        }
    };
        }

        // اگر تا اینجا پاسخی برنگشته، از پیام‌های پیش‌فرض استفاده می‌کنیم
        if (string.IsNullOrEmpty(apiResponse.Message))
        {
            apiResponse.Message = exception.StatusCode switch
            {
                400 => "خطاهای اعتبارسنجی رخ داده است.",
                401 => "دسترسی غیرمجاز است.",
                403 => "دسترسی ممنوع است.",
                404 => "منبع موردنظر یافت نشد.",
                500 => "خطای داخلی سرور. لطفاً بعداً دوباره تلاش کنید.",
                502 => "خطای گیت‌وی نامعتبر. لطفاً بعداً دوباره تلاش کنید.",
                503 => "سرویس در دسترس نیست. لطفاً بعداً دوباره تلاش کنید.",
                504 => "مهلت گیت‌وی به پایان رسیده است. لطفاً بعداً دوباره تلاش کنید.",
                _ => "یک خطای غیرمنتظره رخ داده است. لطفاً بعداً دوباره تلاش کنید."
            };

            // اضافه کردن اطلاعات خطای اصلی به ValidationErrors
            if (apiResponse.ValidationErrors == null || !apiResponse.ValidationErrors.Any())
            {
                apiResponse.ValidationErrors = new List<ValidationError>
        {
            new ValidationError
            {
                PropertyName = "ApiError",
                ErrorMessage = exception.Message
            }
        };
            }

            // در صورتی که پاسخ خطای API در دسترس باشد
            if (exception.Response != null)
            {
                apiResponse.Errors = exception.Response;
            }
            // در غیر این صورت از پیام خطا استفاده می‌کنیم
            else if (!string.IsNullOrEmpty(exception.Message))
            {
                apiResponse.Errors = exception.Message;
            }
        }

        return apiResponse;
    }


    protected bool AddBearerToken()
    {
        var token = _localStorageService.GetStorageValueWithExpiry<string>("token");
        if (token == null)
        {
            //var authenticateVm = new ApiAuthenticateVm
            //{
            //    Email = _apiCredentials.Email,
            //    Password = _apiCredentials.Password
            //};

            //var isAuthenticated = await _apiAuthenticateService.ApiAuthenticateAsync(authenticateVm);
            //if (isAuthenticated)
            //{
            //    token = _localStorageService.GetStorageValueWithExpiry<string>("token");
            //}

            return false;
        }

        _client.HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return true;
    }
}