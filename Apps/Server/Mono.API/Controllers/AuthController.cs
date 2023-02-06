using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mono.CoreService.Filters;
using Mono.CoreService.Interfaces;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;

namespace Mono.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly ICoreServiceManager _coreService;

        public AuthController(ICoreServiceManager coreService, ILoggerManager logger, IMapper mapper) : base(logger, mapper)
        {
            _coreService = coreService;
        }

        [HttpPost("register")]
        [ServiceFilter(typeof(AttributeValidation))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest userRegistration)
        {
            var result = await _coreService.User.RegisterUserAsync(userRegistration);
            return Ok(result);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(AttributeValidation))]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginRequest user)
        {
            var result = await _coreService.User.ValidateUserAsync(user);
            return Ok(result);
        }
    }
}
