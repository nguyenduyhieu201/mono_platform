using Azure.Core;
using Mono.BlazorServer.Services.Interfaces;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Wrapper;

namespace Mono.BlazorServer.Services
{
    public class DocumentService : BaseService, IDocumentService
    {
        public DocumentService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<IResult<string>> CreateDocument(string uri, DocumentRequest request)
        {
            return await PostAsync<string, DocumentRequest>(uri, request);
        }

        public async Task<IResult<string>> UpdateDocument(string uri, DocumentRequest request)
        {
            return await PutAsync<string, DocumentRequest>(uri, request);
        }

        public async Task<SharedLibrary.Wrapper.IResult> DeleteDocument(string uri)
        {
            return await DeleteAsync(uri);
        }

        public async Task<IResult<DocumentDetailResponse>> GetDocumentByGuid(string uri)
        {
            return await GetAsync<DocumentDetailResponse>(uri);
        }

        public async Task<IResult<IList<DocumentTypeResponse>>> GetDocumentTypes(string uri)
        {
            return await GetAsync<IList<DocumentTypeResponse>>(uri);
        }

        public async Task<IResult<IPage<DocumentItemResponse>>> GetPaginatedDocuments(string uri, DocumentFilterRequest filterRequest)
        {
            return await PostAsync<Page<DocumentItemResponse>, DocumentFilterRequest>(uri, filterRequest);
        }
    }
}
