using AutoMapper;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegisterRequest, User>();
            CreateMap<User, UserDto>();
        }
    }
}
