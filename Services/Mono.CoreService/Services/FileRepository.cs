using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Mono.CoreService.Interfaces;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Exceptions;
using Mono.SharedLibrary.Extensions;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.CoreService.Services
{
    public class FileRepository : IFileRepository
    {
        private IWebHostEnvironment _environment;

        public FileRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task DeleteFile(string fileRoute, string containerName)
        {
            throw new NotImplementedException();
        }

        public async Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute, string contentType)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<FileContentResult?>> DownloadFile(string filePath)
        {
            string rootFolderPath = _environment.ContentRootPath;
            string absoluteFilePath = Path.Combine(rootFolderPath, filePath).Replace("\\", "/");
            if (!File.Exists(absoluteFilePath))
                throw new NotFoundException($"File '{filePath}' does not exists");
            var data = await File.ReadAllBytesAsync(absoluteFilePath);
            string mimeType = "application/octet-stream";
            return Result<FileContentResult>.Success(new FileContentResult(data, mimeType)
            {
                FileDownloadName = "avatar.png"
            });
        }

        public async Task<IResult<string>> SaveFile(UploadRequest fileRequest)
        {
            var file = await fileRequest.SaveFile();
            return Result<string>.Success(file.FilePath);
        }
    }
}
