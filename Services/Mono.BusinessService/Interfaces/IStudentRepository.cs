using Mono.SharedLibrary.Models;
using Mono.SharedLibrary.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.BusinessService.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetStudent(int teacherId, int studentId);
        Task<IPage<Student>> GetStudentByPage(int teacherId, int studentId, int pageIndex, int pageSize);
        Task CreateStudentForTeacher(int teacherId, Student student);
        Task UpdateStudent(int teacherId, Student student);
        Task DeleteStudent(Student student);
    }
}
