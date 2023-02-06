using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mono.BusinessService.Interfaces;
using Mono.CoreService.Filters;
using Mono.CoreService.Interfaces;
using Mono.SharedLibrary.Dtos.Requests;

namespace Mono.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentController : BaseApiController
    {
        private readonly IBusinessServiceManager _businessService;

        public DocumentController(IBusinessServiceManager businessService, ILoggerManager logger, IMapper mapper) : base(logger, mapper)
        {
            _businessService = businessService;
        }

        [HttpGet("docType")]
        public async Task<IActionResult> GetDocumentTypes()
        {
            var result = await _businessService.Document.GetDocumentTypes();
            return Ok(result);
        }

        [HttpGet("{docId}")]
        public async Task<IActionResult> GetDocumentByGuid(string docId)
        {
            var result = await _businessService.Document.GetDocumentByGuid(Guid.Parse(docId));
            return Ok(result);
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(AttributeValidation))]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentRequest request)
        {
            var result = await _businessService.Document.CreateDocument(request);
            return Ok(result);
        }

        [HttpPut("update/{docId}")]
        [ServiceFilter(typeof(AttributeValidation))]
        public async Task<IActionResult> UpdateDocument([FromBody] DocumentRequest request, string docId)
        {
            var result = await _businessService.Document.UpdateDocument(request, Guid.Parse(docId));
            return Ok(result);
        }

        [HttpPost("grid/{pageId}")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] DocumentFilterRequest request, int pageId)
        {
            var result = await _businessService.Document.GetDocumentsByPage(request, pageId, 10);
            return Ok(result);
        }

        [HttpDelete("{docId}")]
        public async Task<IActionResult> DeleteDocument(string docId)
        {
            var result = await _businessService.Document.DeleteDocument(Guid.Parse(docId));
            return Ok(result);
        }
    }
}
