using AutoMapper;
using MiniProfiler_Test.Models.Api.Users;
using MiniProfiler_Test.Services.Base;

namespace MiniProfiler_Test.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginRequest, LoginApiVm>().ReverseMap();
        CreateMap<CreateUserDto, CreateUserViewModel>().ReverseMap();
    }
}