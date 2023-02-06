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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeachersController : BaseApiController
    {
        private readonly IBusinessServiceManager _businessService;

        public TeachersController(IBusinessServiceManager businessService, ILoggerManager logger, IMapper mapper) : base(logger, mapper)
        {
            _businessService = businessService;
        }

        [HttpPost]
        [ServiceFilter(typeof(AttributeValidation))]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherCreationDto teacher)
        {
            var teacherdata = _mapper.Map<Teacher>(teacher);
            await _businessService.Teacher.CreateTeacher(teacherdata);
            var teacherReturn = _mapper.Map<TeacherDto>(teacherdata);
            return CreatedAtRoute("TeacherById",
                new
                {
                    teacherId = teacherReturn.Id
                },
                teacherReturn);
        }


        [HttpGet]
        [Authorize]
        [ResponseCache(CacheProfileName = "30SecondsCaching")]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _businessService.Teacher.GetAllTeachers();
            var teachersDto = _mapper.Map<IEnumerable<TeacherDto>>(teachers);
            return Ok(teachersDto);
        }


        [HttpGet("{teacherId}", Name = "TeacherById")]
        public async Task<IActionResult> GetTeacher(int teacherId)
        {
            var teacher = await _businessService.Teacher.GetTeacher(teacherId);
            if (teacher is null)
            {
                _logger.LogInfo($"Teacher with id: {teacherId} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var teacherDto = _mapper.Map<TeacherDto>(teacher);
                return Ok(teacherDto);
            }
        }


        [HttpPut("{teacherId}")]
        [ServiceFilter(typeof(AttributeValidation))]
        [ServiceFilter(typeof(TeacherExistsValidation))]
        public async Task<IActionResult> UpdateTeacher(int teacherId, [FromBody] TeacherCreationDto teacher)
        {
            var teacherData = HttpContext.Items["teacher"] as Teacher;
            _mapper.Map(teacher, teacherData);
            await _businessService.Teacher.UpdateTeacher(teacherData);
            return NoContent();
        }
    }
}
