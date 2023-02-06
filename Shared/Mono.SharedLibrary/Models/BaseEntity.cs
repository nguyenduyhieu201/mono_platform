using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedTime { get; set; }
        [MaxLength(255)]
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        [MaxLength(255)]
        public string? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
