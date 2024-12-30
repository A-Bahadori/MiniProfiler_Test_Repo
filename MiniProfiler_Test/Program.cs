using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MiniProfiler_Test.Interfaces.Api;
using MiniProfiler_Test.Interfaces.Captcha;
using MiniProfiler_Test.Interfaces.Storage;
using MiniProfiler_Test.Mapping;
using MiniProfiler_Test.Services.Api;
using MiniProfiler_Test.Services.Base;
using MiniProfiler_Test.Services.Captcha;
using MiniProfiler_Test.Services.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews(
    options => { options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); });

builder.Services.AddMvc();
builder.Services.AddMiniProfiler()
    .AddEntityFramework();
builder.Services.AddSession();
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 443;
});

#region IoC

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient<IClient, Client>(c => c.BaseAddress = new Uri(builder.Configuration.GetSection("MinimalApiAddress").Value));
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IApiAuthenticateService, ApiAuthenticateService>();
builder.Services.AddScoped<IApiUserService, ApiUserService>();
builder.Services.AddScoped<ICaptchaService, CaptchaService>();

#endregion

#region Authentication

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

#endregion

#region Rate Limiting

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("captchaPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiniProfiler();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name : "areas",
        pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();