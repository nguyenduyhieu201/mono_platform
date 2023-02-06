using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Responses
{
    public class DocumentItemResponse
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public short? Number { get; set; }
        public string? Symbol { get; set; }
        public string? IssuedDeparment { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
