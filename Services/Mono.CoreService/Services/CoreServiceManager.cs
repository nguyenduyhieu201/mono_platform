using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mono.CoreService.Interfaces;
using Mono.Repository.Data;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.CoreService.Services
{
    public class CoreServiceManager : ICoreServiceManager
    {
        private RepositoryContext _repositoryContext;

        private IUserRepository _userRepository;
        private IFileRepository _fileRepository;
        private UserManager<User> _userManager;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;

        public CoreServiceManager(RepositoryContext repositoryContext, UserManager<User> userManager, IMapper mapper, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _repositoryContext = repositoryContext;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _environment = environment;
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository is null)
                    _userRepository = new UserRepository(this, _userManager, _configuration, _mapper);
                return _userRepository;
            }
        }

        public IFileRepository File
        {
            get
            {
                if (_fileRepository is null)
                    _fileRepository = new FileRepository(_environment);
                return _fileRepository;
            }
        }
    }
}
