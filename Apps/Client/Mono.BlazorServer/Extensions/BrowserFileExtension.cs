using Mono.SharedLibrary.Wrapper;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
using Mono.BlazorServer.Shared.Constants;
using Mono.SharedLibrary.Enums;
using Mono.SharedLibrary.Dtos.Requests;
using Azure.Core;
using Mono.SharedLibrary.Extensions;

namespace Mono.BlazorServer.Extensions
{
    public static class BrowserFileExtension
    {
        internal static async Task<byte[]> ToByteArray(this IBrowserFile file)
        {
            using(Stream stream = file.OpenReadStream(maxAllowedSize: FileConstant.MaximumFileSize))
            {
                MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        internal static async Task<UploadRequest> ToUploadRequest(this IBrowserFile file, UploadType uploadType)
        {
            using (Stream stream = file.OpenReadStream(maxAllowedSize: FileConstant.MaximumFileSize))
            {
                MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                return new UploadRequest()
                {
                    FileName = file.Name,
                    Extension = Path.GetExtension(file.Name),
                    UploadType = uploadType,
                    Data = ms.ToArray()
                };
            }
        }

        internal static async Task<UploadRequest[]?> ToUploadRequests(this IList<IBrowserFile> files, UploadType uploadType)
        {
            if (files.Any())
            {
                List<Task<UploadRequest>> tasks = new();
                for (int i = 0; i < files.Count; i++)
                {
                    tasks.Add(files[i].ToUploadRequest(uploadType));
                }
                return await Task.WhenAll(tasks);
            }
            return null;
        }

        internal static FileValidationStatus ValidateFileUpload(this IBrowserFile file, UploadType uploadType)
        {
            if (file.Size > FileConstant.MaximumFileSize)
                return FileValidationStatus.MaximumSizeExceeded;
            else
            {
                var folder = uploadType.ToDescriptionString();
                if (folder.StartsWith("Images"))
                {
                    if (!FileConstant.ImageContentTypes.Contains(file.ContentType))
                        return FileValidationStatus.InvalidExtension;
                }
                else if (!FileConstant.AllowedContentTypes.Contains(file.ContentType))
                    return FileValidationStatus.InvalidExtension;
            }  
            return FileValidationStatus.None;
        }
    }

    enum FileValidationStatus
    {
        MaximumSizeExceeded, InvalidExtension, None
    }
}
