using Mono.SharedLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Models
{
    public class Document : BaseEntity
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public short? Number { get; set; }
        [MaxLength(50)]
        public string? Symbol { get; set; }
        [MaxLength(1000)]
        public string Title { get; set; }
        [MaxLength(1000)]
        [Column(TypeName = "varchar")]
        public string TitleSearch { get; set; }
        public string? Content { get; set; }
        [MaxLength(255)]
        public string? Signer { get; set; }
        [MaxLength(500)]
        public string? IssuedDeparment { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DocumentStatus? Status { get; set; }
        [ForeignKey(nameof(DocumentType))]
        public int DocumentTypeId { get; set; }
        public DocumentType? DocumentType { get; set; }
        public ICollection<DocumentAttachment>? Attachments { get; set; }
    }
}
