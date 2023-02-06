using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mono.CoreService.Filters;
using Mono.CoreService.Interfaces;

namespace Mono.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileController : BaseApiController
    {
        private readonly ICoreServiceManager _coreService;

        public FileController(ICoreServiceManager coreService, ILoggerManager logger, IMapper mapper) : base(logger, mapper)
        {
            _coreService = coreService;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile([FromQuery]string uri)
        {
            var data = await _coreService.File.DownloadFile(uri);
            return Ok(data);
        }
    }
}
