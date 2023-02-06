using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Enums;
using Mono.SharedLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Requests
{
    public class DocumentRequest
    {
        [Display(Name = nameof(DisplayNameResource.doc_number), ResourceType = typeof(DisplayNameResource))]
        [Range(1, short.MaxValue, ErrorMessageResourceName = nameof(ErrorMessageResource.number_min_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public int? Number { get; set; }

        [Display(Name = nameof(DisplayNameResource.doc_symbol), ResourceType = typeof(DisplayNameResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessageResource.string_maxlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? Symbol { get; set; }
       
        [Display(Name = nameof(DisplayNameResource.doc_title), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        [MaxLength(1000, ErrorMessageResourceName = nameof(ErrorMessageResource.string_maxlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string Title { get; set; }

        [Display(Name = nameof(DisplayNameResource.doc_signer), ResourceType = typeof(DisplayNameResource))]
        [MaxLength(255, ErrorMessageResourceName = nameof(ErrorMessageResource.string_maxlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? Signer { get; set; }

        [Display(Name = nameof(DisplayNameResource.doc_issued_dept), ResourceType = typeof(DisplayNameResource))]
        [MaxLength(500, ErrorMessageResourceName = nameof(ErrorMessageResource.string_maxlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? IssuedDeparment { get; set; }

        public string? Content { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int DocumentTypeId { get; set; }

        public string? CreatedBy { get; set; }

        public string? LastModifiedBy { get; set; }

        public UploadRequest[]? Attachments { get; set; }

        public int[]? FilesToDelete { get; set; }
    }
}
