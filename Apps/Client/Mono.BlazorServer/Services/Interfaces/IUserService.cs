using Microsoft.AspNetCore.Mvc;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Wrapper;
using System.Security.Claims;

namespace Mono.BlazorServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IResult<UserDto>> GetUserByUsername(string username);
        Task<SharedLibrary.Wrapper.IResult> UpdateUserAsync(UpdateProfileRequest updateRequest);
        Task<IResult<UploadFileResponse>> UpdateProfilePicture(UploadRequest fileRequest, string userName);
    }
}
