using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.BusinessService.Interfaces;
using Mono.CoreService.Interfaces;

namespace Mono.BusinessService.Filters
{
    public class TeacherExistsValidation : IAsyncActionFilter
    {
        private readonly IBusinessServiceManager _repository;
        private readonly ILoggerManager _logger;

        public TeacherExistsValidation(IBusinessServiceManager repository, ILoggerManager logger)
        {
            _repository = repository; _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT")!;

            var id = (int)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id") || x.Equals("teacherId")).SingleOrDefault()];

            var teacher = await _repository.Teacher.GetTeacher(id);
            if (teacher is null)
            {
                _logger.LogInfo($"Teacher with id: {id} doesn't exist in the database.");
                var response = new ObjectResult(new ResponseModel
                {
                    StatusCode = 404,
                    Message = $"Teacher with id: {id} doesn't exist in the database."
                });
                context.Result = response;
            }
            else
            {
                context.HttpContext.Items.Add("teacher", teacher);
                await next();
            }
        }
    }
}
