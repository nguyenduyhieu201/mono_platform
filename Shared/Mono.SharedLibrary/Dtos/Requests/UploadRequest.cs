using Mono.SharedLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Requests
{
    public class UploadRequest
    {
        public string? FileName { get; set; }
        public string? Extension { get; set; }
        public UploadType UploadType { get; set; }
        public byte[]? Data { get; set; }
    }
}
