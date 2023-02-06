using AutoMapper;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Dtos.Responses;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Mappings
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<DocumentType, DocumentTypeResponse>();
            CreateMap<DocumentRequest, Document>()
                .ForMember(x => x.Attachments, options => options.Ignore());
            CreateMap<Document, DocumentRequest>()
                .ForMember(x => x.Attachments, options => options.Ignore())
                .ForMember(x => x.FilesToDelete, options => options.Ignore());
            CreateMap<Document, DocumentDetailResponse>();
            CreateMap<Document, DocumentItemResponse>();
        }
    }
}
