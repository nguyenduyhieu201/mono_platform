using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.CoreService.Interfaces
{
    public interface IUserRepository
    {
        Task<IResult<IdentityResult>> RegisterUserAsync(UserRegisterRequest userForRegistration);
        Task<IResult<UserLoginResponse>> ValidateUserAsync(UserLoginRequest loginDto);
        Task<IResult<UserDto>> GetUserByUsername(string username);
        Task<IResult<UploadFileResponse>> UpdateProfilePicture(UploadRequest request, string userName);
    }
}
