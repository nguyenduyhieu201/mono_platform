using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Responses
{
    public class DocumentTypeResponse
    {
        public int Id { get; set; }
        public string Title_en { get; set; }
        public string Title_vi { get; set; }
    }
}
