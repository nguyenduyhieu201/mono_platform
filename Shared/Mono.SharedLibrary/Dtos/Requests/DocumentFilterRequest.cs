using Mono.SharedLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Requests
{
    public class DocumentFilterRequest
    {
        public int? DocumentTypeId { get; set; }

        [Display(Name = nameof(DisplayNameResource.doc_number), ResourceType = typeof(DisplayNameResource))]
        [Range(1, short.MaxValue, ErrorMessageResourceName = nameof(ErrorMessageResource.number_min_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public short? Number { get; set; }

        [Display(Name = nameof(DisplayNameResource.doc_symbol), ResourceType = typeof(DisplayNameResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessageResource.string_maxlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? Symbol { get; set; }

        [Display(Name = nameof(DisplayNameResource.doc_title), ResourceType = typeof(DisplayNameResource))]
        [MaxLength(1000, ErrorMessageResourceName = nameof(ErrorMessageResource.string_maxlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? Title { get; set; }
    }
}
