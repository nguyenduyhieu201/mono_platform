using Mono.BlazorServer.Extensions;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Wrapper;
using System.Net.Http.Json;
using IResult = Mono.SharedLibrary.Wrapper.IResult;

namespace Mono.BlazorServer.Services
{
    public abstract class BaseService
    {
        private readonly HttpClient _httpClient;

        public BaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult> GetAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode) return Result.Fail();
            return Result.Success();
        }

        public async Task<IResult<T>> GetAsync<T>(string uri) // T: Response type
        {
            var response = await _httpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode) return Result<T>.Fail();
            var result = await response.ToResult<T>();
            return result;
        }

        public async Task<IResult<T>> PostAsync<T, T1>(string uri, T1 request) // T: Response type, T1: Request type
        {
            var response = await _httpClient.PostAsJsonAsync(uri, request);
            if (!response.IsSuccessStatusCode) return Result<T>.Fail();
            var result = await response.ToResult<T>();
            return result;
        }

        public async Task<IResult> PostAsync<T1>(string uri, T1 request) // T1: Request type
        {
            var response = await _httpClient.PostAsJsonAsync(uri, request);
            if (!response.IsSuccessStatusCode) return Result.Fail();
            return Result.Success();
        }

        public async Task<IResult<T>> PutAsync<T, T1>(string uri, T1 request) // T: Response type, T1: Request type
        {
            var response = await _httpClient.PutAsJsonAsync(uri, request);
            if (!response.IsSuccessStatusCode) return Result<T>.Fail();
            var result = await response.ToResult<T>();
            return result;
        }

        public async Task<IResult> PutAsync<T1>(string uri, T1 request) // T1: Request type
        {
            var response = await _httpClient.PutAsJsonAsync(uri, request);
            if (!response.IsSuccessStatusCode) return Result.Fail();
            return Result.Success();
        }

        public async Task<IResult> DeleteAsync(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            if (!response.IsSuccessStatusCode) return Result.Fail();
            return Result.Success();
        }
    }
}
