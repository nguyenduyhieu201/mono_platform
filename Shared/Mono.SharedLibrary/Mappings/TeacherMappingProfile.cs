using AutoMapper;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Mappings
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, StudentDto>();

            CreateMap<StudentCreationDto, Student>();

            CreateMap<StudentUpdateDto, Student>().ReverseMap();
        }
    }
}
