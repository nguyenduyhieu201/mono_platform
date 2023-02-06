using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Wrapper;
using IResult = Mono.SharedLibrary.Wrapper.IResult;

namespace Mono.BlazorServer.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<IResult<IList<DocumentTypeResponse>>> GetDocumentTypes(string uri);
        Task<IResult<DocumentDetailResponse>> GetDocumentByGuid(string uri);
        Task<IResult<string>> CreateDocument(string uri, DocumentRequest request);
        Task<IResult<string>> UpdateDocument(string uri, DocumentRequest request);
        Task<IResult<IPage<DocumentItemResponse>>> GetPaginatedDocuments(string uri, DocumentFilterRequest filterRequest);
        Task<IResult> DeleteDocument(string uri);
    }
}
