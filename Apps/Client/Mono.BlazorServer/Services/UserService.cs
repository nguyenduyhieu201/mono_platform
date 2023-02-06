using Azure.Core;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mono.BlazorServer.Extensions;
using Mono.BlazorServer.Services.Interfaces;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Wrapper;
using System.Text.Json;

namespace Mono.BlazorServer.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(HttpClient httpClient) : base(httpClient) { }

        public async Task<IResult<UserDto>> GetUserByUsername(string username)
        {
            return await GetAsync<UserDto>("/api/user/" + username);
        }

        public async Task<IResult<UploadFileResponse>> UpdateProfilePicture(UploadRequest fileRequest, string userName)
        {
            return await PostAsync<UploadFileResponse, UploadRequest>("/api/user/" + userName + "/avatar", fileRequest);
        }

        public async Task<SharedLibrary.Wrapper.IResult> UpdateUserAsync(UpdateProfileRequest updateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
