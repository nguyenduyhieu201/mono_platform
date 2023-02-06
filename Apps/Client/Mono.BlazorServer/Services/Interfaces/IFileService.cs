using Microsoft.AspNetCore.Mvc;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Wrapper;

namespace Mono.BlazorServer.Services.Interfaces
{
    public interface IFileService
    {
        Task<IResult<DownloadFileResponse>> GetFileInfo(string uri);
    }
}
