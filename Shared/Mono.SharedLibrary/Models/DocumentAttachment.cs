using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Models
{
    public class DocumentAttachment
    {
        public int Id { get; set; }
        [ForeignKey(nameof(File))]
        public int FileId { get; set; }
        public File? File { get; set; }
        public int DocumentId { get; set; }
        public Document? Document { get; set; }
    }
}
