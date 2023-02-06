using Microsoft.AspNetCore.Components.Authorization;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Wrapper;
using System.Security.Claims;

namespace Mono.BlazorServer.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal> GetUserInfo();
        Task<IResult<UserLoginResponse>> Login(UserLoginRequest loginRequest);
        Task<SharedLibrary.Wrapper.IResult> Register(UserRegisterRequest registerRequest);
        Task Logout();
    }
}
