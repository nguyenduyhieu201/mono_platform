using Microsoft.AspNetCore.Mvc;
using Mono.BlazorServer.Extensions;
using Mono.BlazorServer.Services.Interfaces;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Wrapper;
using System.Net.Http;
using System.Text.Json;

namespace Mono.BlazorServer.Services
{
    public class FileService : BaseService, IFileService
    {
        public FileService(HttpClient httpClient) : base(httpClient) { }

        public async Task<IResult<DownloadFileResponse>> GetFileInfo(string uri)
        {
            return await GetAsync<DownloadFileResponse>(uri);
        }
    }
}
