using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Repository.Data
{
    public class StudentData : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasData(
                new Student
                {
                    Id = 1,
                    Name = "Azeez",
                    Class = "Science",
                    TeacherId = 1
                },

                new Student
                {
                    Id = 2,
                    Name = "Kamal",
                    Class = "Management",
                    TeacherId = 2
                },

                new Student
                {
                    Id = 3,
                    Name = "Benson",
                    Class = "Science",
                    TeacherId = 1
                });
        }
    }

}
