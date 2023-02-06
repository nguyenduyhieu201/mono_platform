using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Responses
{
    public class DownloadFileResponse
    {
        public byte[] fileContents { get; set; }
        public string contentType { get; set; }
        public string fileDownloadName { get; set; }
        public DateTimeOffset? lastModified { get; set; }
    }
}
