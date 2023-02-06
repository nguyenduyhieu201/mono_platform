using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace Mono.BlazorServer.Handlers.RewriteRules
{
    public class CustomRedirectRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var host = request.Host;
            //if (host.Host.Contains("yourSampleHost", StringComparison.OrdinalIgnoreCase))
            //{
            //    if (host.Port == 8080)
            //    {
            //        context.Result = RuleResult.ContinueRules;
            //        return;
            //    }
            //}
            //var response = context.HttpContext.Response;
            //response.StatusCode = (int)HttpStatusCode.BadRequest;
            //context.Result = RuleResult.EndResponse;
            if (request.Path == "/")
            {
                var response = context.HttpContext.Response;
                response.StatusCode = (int)HttpStatusCode.MovedPermanently;
                response.Headers[HeaderNames.Location] = "/profile";
                context.Result = RuleResult.EndResponse;
            }
            else
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }
        }
    }
}
