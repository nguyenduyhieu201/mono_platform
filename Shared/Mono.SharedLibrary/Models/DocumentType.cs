using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Models
{
    public class DocumentType
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        [MaxLength(255)]
        public string Title_en { get; set; }
        [MaxLength(255)]
        public string Title_vi { get; set; }
    }
}
