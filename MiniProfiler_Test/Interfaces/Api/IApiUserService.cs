using MiniProfiler_Test.Models.Api.Users;
using MiniProfiler_Test.Services.Base;

namespace MiniProfiler_Test.Interfaces.Api;

public interface IApiUserService
{
    Task<Response<SearchUsersResult>> GetAllUsersAsync(string username, string firstname, string lastname, string sort,
        int pageNumber = 1, int pageSize = 1, string? persianStartDate = null, string? persianEndDate = null);

    Task<Response<UserDto>> GetUserByIdAsync(int userId);
    Task<Response<UserDto>> CreateUserAsync(CreateUserViewModel createUserViewModel);
    // Task<Response<bool>> UpdateUserAsync(long userId, UpdateUserViewModel updateUserViewModel);
    Task<Response<bool>> DeleteUserAsync(int userId);
    // Task<Response<bool>> AddUserToRoleAsync(long userId, int roleId);
    // Task<Response<bool>> RemoveUserFromRoleAsync(long userId, int roleId);
}