using Azure;
using Microsoft.AspNetCore.Components.Authorization;
using Mono.BlazorServer.Extensions;
using Mono.BlazorServer.Services.Interfaces;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Wrapper;
using System.Security.Claims;
using System.Text.Json;
using IResult = Mono.SharedLibrary.Wrapper.IResult;
using Mono.BlazorServer.Handlers.Authentication;

namespace Mono.BlazorServer.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) : base(httpClient)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ClaimsPrincipal> GetUserInfo()
        {
            var state = await ((CustomMembershipProvider)_authenticationStateProvider).GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResult<UserLoginResponse>> Login(UserLoginRequest loginRequest)
        {
            return await PostAsync<UserLoginResponse, UserLoginRequest>("/api/auth/login", loginRequest);
        }

        public async Task Logout() =>
            await ((CustomMembershipProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

        public async Task<IResult> Register(UserRegisterRequest registerRequest)
        {
            return await PostAsync<UserRegisterRequest>("/api/auth/register", registerRequest);;
        }
    }
}
