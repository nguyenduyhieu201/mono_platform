using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mono.CoreService.Interfaces;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Exceptions;
using Mono.SharedLibrary.Models;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mono.CoreService.Services
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly ICoreServiceManager _coreService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private User? _user;

        public UserRepository(ICoreServiceManager coreService, UserManager<User> userManager, IConfiguration configuration, IMapper mapper)
        {
            _coreService = coreService;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IResult<IdentityResult>> RegisterUserAsync(UserRegisterRequest userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);
            user.CreatedTime = DateTime.Now;
            user.CreatedBy = "system";
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            return Result<IdentityResult>.Success(result);
        }

        public async Task<IResult<UserLoginResponse>> ValidateUserAsync(UserLoginRequest loginDto)
        {
            _user = await _userManager.FindByNameAsync(loginDto.UserName);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            if (!result) throw new UnauthorizedException("Username or password was wrong");
            string jwtToken = await GenerateToken();
            return Result<UserLoginResponse>.Success(new UserLoginResponse { Token = jwtToken });
        }

        #region private auth methods
        public async Task<string> GenerateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _user.UserName??""),
                new Claim(ClaimTypes.Name, _user.FirstName??""),
                new Claim(ClaimTypes.Surname, _user.LastName??""),
                new Claim(ClaimTypes.Email, _user.Email??""),
                new Claim(ClaimTypes.Uri, _user.ProfilePictureUrl??"")
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
        #endregion

        public async Task<IResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) throw new NotFoundException($"Username {username} does not exists");
            var userDto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Success(userDto);
        }

        public async Task<IResult<UploadFileResponse>> UpdateProfilePicture(UploadRequest request, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) throw new NotFoundException("User not found");
            if (request.Data != null)
            {
                var filePath = await _coreService.File.SaveFile(request);
                user.ProfilePictureUrl = filePath.Data;
                var identityResult = await _userManager.UpdateAsync(user);
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                if (!identityResult.Succeeded) 
                    throw new BadRequestException("Update user failed: " + string.Join("\n", errors));
                return Result<UploadFileResponse>.Success(
                    new UploadFileResponse { fileContents = request.Data, filePath = filePath.Data }
                );
            }
            else
            {
                user.ProfilePictureUrl = null;
                var identityResult = await _userManager.UpdateAsync(user);
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                if (!identityResult.Succeeded) 
                    throw new BadRequestException("Update user failed: " + string.Join("\n", errors));
                return Result<UploadFileResponse>.Success(
                    new UploadFileResponse { fileContents = null, filePath = string.Empty }
                );
            }
        }
    }
}
