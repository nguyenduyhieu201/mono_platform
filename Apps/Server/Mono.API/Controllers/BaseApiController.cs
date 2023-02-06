using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mono.CoreService.Interfaces;

namespace Mono.API.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected readonly ILoggerManager _logger;
        protected readonly IMapper _mapper;

        public BaseApiController(ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
    }
}
