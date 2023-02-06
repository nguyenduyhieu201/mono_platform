using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Repository.Data
{
    public class DocTypeData : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.HasData(
                new DocumentType
                {
                    Id = 1,
                    Guid = Guid.NewGuid(),
                    Title_vi = "Tờ trình",
                    Title_en = "Report"
                },
                new DocumentType
                {
                    Id = 2,
                    Guid = Guid.NewGuid(),
                    Title_vi = "Công văn",
                    Title_en = "Dispatch"
                },
                new DocumentType
                {
                    Id = 3,
                    Guid = Guid.NewGuid(),
                    Title_vi = "Quyết định",
                    Title_en = "Decision"
                },
                new DocumentType
                {
                    Id = 4,
                    Guid = Guid.NewGuid(),
                    Title_vi = "Nghị quyết",
                    Title_en = "Resolution"
                },
                new DocumentType
                {
                    Id = 5,
                    Guid = Guid.NewGuid(),
                    Title_vi = "Chỉ thị",
                    Title_en = "Command"
                });
        }
    }
}
