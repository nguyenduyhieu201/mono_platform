using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Mono.CoreService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.CoreService.Filters
{
    public class AttributeValidation : IActionFilter
    {
        private readonly ILoggerManager _logger;

        public AttributeValidation(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            //var param = context.ActionArguments
            //    .SingleOrDefault(x => x.Value.ToString()
            //    .Contains("Request")).Value;

            //if (param is null)
            //{
            //    _logger.LogError($"Object sent from client is null. Controller: {controller}, " +
            //        $"action: {action}");

            //    context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
            //    return;
            //}
            if (!context.ModelState.IsValid)
            {
                _logger.LogError($"Invalid model state for the object. Controller: {controller}, action: {action}");
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }

}
