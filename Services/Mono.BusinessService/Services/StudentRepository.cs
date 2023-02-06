using Microsoft.EntityFrameworkCore;
using Mono.BusinessService.Interfaces;
using Mono.Repository.Data;
using Mono.Repository.GenericRepository.Service;
using Mono.SharedLibrary.Models;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.BusinessService.Services
{
    public class StudentRepository : RepositoryBase, IStudentRepository
    {
        public StudentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateStudentForTeacher(int teacherId, Student student)
        {
            student.TeacherId = teacherId;
            await CreateAsync<Student>(student);
        }

        public async Task UpdateStudent(int teacherId, Student student)
        {
            student.TeacherId = teacherId;
            await UpdateAsync<Student>(student);
        }

        public async Task DeleteStudent(Student student) => await DeleteAsync<Student>(student);

        public async Task<Student?> GetStudent(int teacherId, int studentId)
            => await GetByCondition<Student>(e => e.TeacherId.Equals(teacherId) && e.Id.Equals(studentId)).SingleOrDefaultAsync();

        public async Task<IPage<Student>> GetStudentByPage(int teacherId, int studentId, int pageIndex, int pageSize)
        {
            //var query = GetByCondition<Student>(e => e.TeacherId.Equals(teacherId) && e.Id.Equals(studentId)).OrderByDescending(e => e.TeacherId);
            //return await PaginateAsync(query, pageIndex, pageSize);
            throw new NotImplementedException();
        }
    }
}
