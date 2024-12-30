using AutoMapper;
using MiniProfiler_Test.Convertors;
using MiniProfiler_Test.Interfaces.Api;
using MiniProfiler_Test.Interfaces.Storage;
using MiniProfiler_Test.Models.Api.Users;
using MiniProfiler_Test.Services.Base;

namespace MiniProfiler_Test.Services.Api;

public class ApiUserService : BaseHttpService, IApiUserService
{
    private readonly IMapper _mapper;

    #region Constractor

    public ApiUserService(IClient client, ILocalStorageService localStorageService, IMapper mapper)
        : base(client, localStorageService)
    {
        _mapper = mapper;
    }

    #endregion

    public async Task<Response<SearchUsersResult>> GetAllUsersAsync(string username, string firstname, string lastname,
        string sort, int pageNumber = 1, int pageSize = 1, string? persianStartDate = null,
        string? persianEndDate = null)
    {
        DateTime? startDateGregorian =
            string.IsNullOrWhiteSpace(persianStartDate) ? null : persianStartDate.ToGregorianDateTime();
        DateTime? endDateGregorian = string.IsNullOrWhiteSpace(persianEndDate)
            ? null
            : persianEndDate.ToGregorianDateTime()?.Date.Add(new TimeSpan(23, 59, 59));

        try
        {
            if (!AddBearerToken())
            {
                //TODO Get token again
            }

            var response = new Response<SearchUsersResult>();

            var searchUserDto = new SearchUserDto()
            {
                FirstName = firstname?.Trim(),
                LastName = lastname?.Trim(),
                Username = username?.Trim(),
                Role = null,
                IsDeleted = false,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortBy = sort,
                SortDescending = true,
                CreatedFromDate = startDateGregorian,
                CreatedToDate = endDateGregorian
            };

            var result = await _client.SearchUsersAsync(searchUserDto);

            if (!result.IsSuccess)
            {
                response.Success = false;
                response.Errors = result.Error;
                response.Data = null;

                return response;
            }

            response.Success = true;
            response.Data = result.Data;
            response.StatusCode = 200;

            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<SearchUsersResult>(ex);
        }
    }

    public async Task<Response<UserDto>> GetUserByIdAsync(int userId)
    {
        try
        {
            if (!AddBearerToken())
            {
                //TODO Get token again
            }

            var response = new Response<UserDto>();

            var result = await _client.GetUserByIdAsync(userId);

            response.Success = true;
            response.Data = result.Data;
            response.StatusCode = 200;

            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<UserDto>(ex);
        }
    }

    public async Task<Response<UserDto>> CreateUserAsync(CreateUserViewModel createUserViewModel)
    {
        try
        {
            if (!AddBearerToken())
            {
                //TODO Get token again
            }

            var response = new Response<UserDto>();
            var createUserModel = _mapper.Map<CreateUserDto>(createUserViewModel);
            var createUserResult = await _client.CreateUserAsync(createUserModel);

            if (!createUserResult.IsSuccess)
            {
                response.Success = false;
                response.Errors = createUserResult.Error;
                response.Data = null;
                return response;
            }

            response.Success = true;
            response.Data = createUserResult.Data;
            response.StatusCode = 200;

            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<UserDto>(ex);
        }
    }
    
    public async Task<Response<bool>> DeleteUserAsync(int userId)
    {
        try
        {
            if (!AddBearerToken())
            {
                //TODO Get token again
            }

            var response = new Response<bool>();

            var result = await _client.DeleteUserAsync(userId);

            if (!result.IsSuccess)
            {
                response.Success = false;
                response.Errors = result.Error;
                response.Data = false;

                return response;
            }

            response.Success = true;
            response.Data = result.Data;
            response.StatusCode = 200;
            
            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<bool>(ex);
        }
    }
}