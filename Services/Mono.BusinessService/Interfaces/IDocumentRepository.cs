using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.BusinessService.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IResult<IList<DocumentTypeResponse>>> GetDocumentTypes();
        Task<IResult<string>> CreateDocument(DocumentRequest request);
        Task<IResult> UpdateDocument(DocumentRequest request, Guid id);
        Task<IResult<IPage<DocumentItemResponse>>> GetDocumentsByPage(DocumentFilterRequest request, int pageIndex, int pageSize);
        Task<IResult<DocumentDetailResponse>> GetDocumentByGuid(Guid id);
        Task<IResult> DeleteDocument(Guid id);
    }
}
