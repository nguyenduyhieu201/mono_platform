using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Responses
{
    public class UploadFileResponse
    {
        public byte[]? fileContents { get; set; }
        public string filePath { get; set; }
    }
}
