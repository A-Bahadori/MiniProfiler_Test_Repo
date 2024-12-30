using MiniProfiler_Test.Interfaces.Api;
using MiniProfiler_Test.Interfaces.Storage;
using MiniProfiler_Test.Models.Api.Users;
using MiniProfiler_Test.Services.Base;

namespace MiniProfiler_Test.Services.Api;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

public class ApiAuthenticateService : BaseHttpService, IApiAuthenticateService
{
    #region Constractor
    private readonly ILocalStorageService _localStorageService;
    private IHttpContextAccessor _httpContextAccessor;
    private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly IMapper _mapper;

    public ApiAuthenticateService(IClient client, ILocalStorageService localStorageService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        : base(client, localStorageService)
    {
        _localStorageService = localStorageService;
        _httpContextAccessor = httpContextAccessor;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        _mapper = mapper;
    }
    #endregion
    
    public async Task<Response<UserDto>> LoginApiAsync(LoginApiVm loginApiVm)
    {
        try
        {
            var loginApiModel = _mapper.Map<LoginRequest>(loginApiVm);
            var response = new Response<UserDto>();

            var loginResponse = await _client.LoginUserAsync(loginApiModel);
            if (loginResponse.AccessToken == string.Empty)
            {
                response.Success = false;
                response.Errors = "خطا در احراز هویت";
                return response;
            }

            response.Success = true;
            response.Data = loginResponse.UserDto;
            response.StatusCode = 200;
            
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(loginResponse.AccessToken);
            var claims = ParseClaims(tokenContent);
            await SignInUserAsync(claims);

            _localStorageService.ClearStorage(["token"]);
            _localStorageService.SetStorageValueWithExpiry("token", loginResponse.AccessToken, TimeSpan.FromMinutes(60));

            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<UserDto>(ex);
        }
    }

    public async Task LogoutAsync()
    {
        _localStorageService.ClearStorage(["token"]);
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private IList<Claim> ParseClaims(JwtSecurityToken token)
    {
        var claims = token.Claims.ToList();
        // claims.Add(new Claim(ClaimTypes.Name, token.Subject));
        return claims;
    }

    private async Task SignInUserAsync(IList<Claim> claims)
    {
        // var roleClaims = claims.Where(c => c.Type is "role" or ClaimTypes.Role).ToList();
        // foreach (var roleClaim in roleClaims)
        // {
        //     claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
        // }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

}