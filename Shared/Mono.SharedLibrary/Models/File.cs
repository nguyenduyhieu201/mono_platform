using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Models
{
    public class File : BaseEntity
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        [MaxLength(255)]
        public string FileName { get; set; }
        [MaxLength(1000)]
        public string FilePath { get; set; }
        [MaxLength(1000)]
        public string? PdfConvertedPath { get; set; }
        [MaxLength(10)]
        public string? Extension { get; set; }
        public long FileSize { get; set; }
    }
}
