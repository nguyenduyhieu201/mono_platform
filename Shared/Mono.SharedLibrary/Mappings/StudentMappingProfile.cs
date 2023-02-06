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
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
            CreateMap<Teacher, TeacherDto>();

            CreateMap<TeacherCreationDto, Teacher>();

            CreateMap<TeacherUpdateDto, Teacher>().ReverseMap();
        }
    }
}
