using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Mono.CoreService.Interfaces;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.BusinessService.Interfaces;

namespace Mono.BusinessService.Filters
{
    public class StudentExistsValidation : IAsyncActionFilter
    {
        private readonly IBusinessServiceManager _repository;
        private readonly ILoggerManager _logger;

        public StudentExistsValidation(IBusinessServiceManager repository, ILoggerManager logger)
        {
            _repository = repository; _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") ? true : false;

            var teacherId = (int)context.ActionArguments["teacherId"];
            var teacher = await _repository.Teacher.GetTeacher(teacherId);

            if (teacher is null)
            {
                _logger.LogInfo($"Teacher with id: {teacherId} doesn't exist in the database.");
                var response = new ObjectResult(new ResponseModel
                {
                    StatusCode = 404,
                    Message = $"Teacher with id: {teacherId} doesn't exist in the database."
                });
                context.Result = response;
                return;
            }
            var id = (int)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id") || x.Equals("studentId")).SingleOrDefault()];
            var student = await _repository.Student.GetStudent(teacherId, id);

            if (student == null)
            {
                _logger.LogInfo($"Student with id: {id} doesn't exist in the database.");
                var response = new ObjectResult(new ResponseModel
                {
                    StatusCode = 404,
                    Message = $"Student with id: {id} doesn't exist in the database."
                });
                context.Result = response;
            }
            else
            {
                context.HttpContext.Items.Add("student", student);
                await next();
            }
        }
    }
}
