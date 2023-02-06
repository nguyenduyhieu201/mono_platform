using Microsoft.AspNetCore.Mvc;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.CoreService.Interfaces
{
    public interface IFileRepository
    {
        Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute, string contentType);
        Task DeleteFile(string fileRoute, string containerName);
        Task<IResult<FileContentResult?>> DownloadFile(string filePath);
        Task<IResult<string>> SaveFile(UploadRequest fileRequest);
    }
}
