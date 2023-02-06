using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.BusinessService.Interfaces
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetAllTeachers();
        Task<Teacher> GetTeacher(int teacherId);
        Task CreateTeacher(Teacher teacher);
        Task UpdateTeacher(Teacher teacher);
    }
}
