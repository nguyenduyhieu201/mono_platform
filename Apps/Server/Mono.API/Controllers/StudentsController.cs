using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mono.BusinessService.Filters;
using Mono.BusinessService.Interfaces;
using Mono.CoreService.Filters;
using Mono.CoreService.Interfaces;
using Mono.SharedLibrary.Dtos;
using Mono.SharedLibrary.Models;

namespace Mono.API.Controllers
{
    [Route("api/teachers/{teacherId}/students")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StudentsController : BaseApiController
    {
        private readonly IBusinessServiceManager _businessService;

        public StudentsController(IBusinessServiceManager businessService, ILoggerManager logger, IMapper mapper) : base(logger, mapper)
        {
            _businessService = businessService;
        }

        [HttpPost]
        [ServiceFilter(typeof(AttributeValidation))]
        [ServiceFilter(typeof(TeacherExistsValidation))]
        public async Task<IActionResult> CreateStudentForTeacher(int teacherId, [FromBody] StudentCreationDto student)
        {
            var studentData = _mapper.Map<Student>(student);
            await _businessService.Student.CreateStudentForTeacher(teacherId, studentData);
            var studentReturn = _mapper.Map<StudentDto>(studentData);
            return Ok(studentReturn);
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(AttributeValidation))]
        [ServiceFilter(typeof(StudentExistsValidation))]
        public async Task<IActionResult> UpdateStudentForTeacher(int teacherId, int id, [FromBody] StudentUpdateDto student)
        {
            var studentData = HttpContext.Items["student"] as Student;
            _mapper.Map(student, studentData);
            await _businessService.Student.UpdateStudent(teacherId, studentData);
            return NoContent();
        }


        [HttpGet]
        [ServiceFilter(typeof(StudentExistsValidation))]
        public async Task<IActionResult> GetStudent(int teacherId, int studentId)
        {
            var student = await _businessService.Student.GetStudent(teacherId, studentId);
            var studentDto = _mapper.Map<StudentDto>(student);
            return Ok(studentDto);
        }
    }
}
