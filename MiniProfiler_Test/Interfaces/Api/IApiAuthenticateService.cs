using MiniProfiler_Test.Models.Api.Users;
using MiniProfiler_Test.Services.Base;

namespace MiniProfiler_Test.Interfaces.Api;

public interface IApiAuthenticateService
{
    Task<Response<UserDto>> LoginApiAsync(LoginApiVm loginApiVm);
    Task LogoutAsync();
}