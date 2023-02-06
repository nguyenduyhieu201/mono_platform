using Microsoft.AspNetCore.Mvc;

namespace Mono.BlazorServer.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet("[action]")]
        public IActionResult Download(string FileName)
        {
            string path = Path.Combine(
                                _environment.WebRootPath,
                                "files",
                                FileName);

            var stream = new FileStream(path, FileMode.Open);

            var result = new FileStreamResult(stream, "text/plain");
            result.FileDownloadName = FileName;
            return result;
        }
    }
}
