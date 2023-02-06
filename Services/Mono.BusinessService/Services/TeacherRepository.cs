using Microsoft.EntityFrameworkCore;
using Mono.BusinessService.Interfaces;
using Mono.Repository.Data;
using Mono.Repository.GenericRepository.Service;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.BusinessService.Services
{
    public class TeacherRepository : RepositoryBase, ITeacherRepository
    {
        public TeacherRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateTeacher(Teacher teacher) => await CreateAsync<Teacher>(teacher);

        public async Task UpdateTeacher(Teacher teacher) => await UpdateAsync<Teacher>(teacher);

        public async Task<IEnumerable<Teacher>> GetAllTeachers()
        {
            return await GetAll<Teacher>().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Teacher?> GetTeacher(int teacherId) => await GetByIdAsync<Teacher>(teacherId);
    }
}
