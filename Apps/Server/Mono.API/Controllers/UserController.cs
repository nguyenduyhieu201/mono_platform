using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mono.CoreService.Filters;
using Mono.CoreService.Interfaces;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;

namespace Mono.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseApiController
    {
        private readonly ICoreServiceManager _coreService;

        public UserController(ICoreServiceManager coreService, ILoggerManager logger, IMapper mapper) : base(logger, mapper)
        {
            _coreService = coreService;
        }

        [HttpGet("{username}")]
        [ServiceFilter(typeof(AttributeValidation))]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var userInfo = await _coreService.User.GetUserByUsername(username);
            return Ok(userInfo);
        }

        [HttpPost("{username}/avatar")]
        [ServiceFilter(typeof(AttributeValidation))]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] UploadRequest request, string username)
        {
            var fileInfo = await _coreService.User.UpdateProfilePicture(request, username);
            return Ok(fileInfo);
        }
    }
}
