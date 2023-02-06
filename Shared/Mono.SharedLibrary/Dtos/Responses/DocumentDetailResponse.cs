using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mono.SharedLibrary.Enums;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Responses
{
    public class DocumentDetailResponse
    {
        public Guid Guid { get; set; }
        public short? Number { get; set; }
        public string? Symbol { get; set; }
        public string Title { get; set; }
        public string TitleSearch { get; set; }
        public string? Content { get; set; }
        public string? Signer { get; set; }
        public string? IssuedDeparment { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
        public FileDto[]? Attachments { get; set; }
    }

    public class FileDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string FileName { get; set; }
    }
}
