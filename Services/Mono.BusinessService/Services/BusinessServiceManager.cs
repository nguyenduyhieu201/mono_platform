using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mono.BusinessService.Interfaces;
using Mono.Repository.Data;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.BusinessService.Services
{
    public class BusinessServiceManager : IBusinessServiceManager
    {
        private RepositoryContext _repositoryContext;

        private ITeacherRepository _teacherRepository;
        private IStudentRepository _studentRepository;
        private IDocumentRepository _documentRepository;
        private UserManager<User> _userManager;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public BusinessServiceManager(RepositoryContext repositoryContext, UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            _repositoryContext = repositoryContext;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public ITeacherRepository Teacher
        {
            get
            {
                if (_teacherRepository is null)
                    _teacherRepository = new TeacherRepository(_repositoryContext);
                return _teacherRepository;
            }
        }
        public IStudentRepository Student
        {
            get
            {
                if (_studentRepository is null)
                    _studentRepository = new StudentRepository(_repositoryContext);
                return _studentRepository;
            }
        }

        public IDocumentRepository Document
        {
            get
            {
                if (_documentRepository is null)
                    _documentRepository = new DocumentRepository(_mapper, _repositoryContext);
                return _documentRepository;
            }
        }
    }
}
