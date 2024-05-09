using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace fyiReporting.ReportServerMvc.Controllers
{
    public class IsValidRequestAttribute : ActionFilterAttribute
    {
        private readonly string _tag;

        public IsValidRequestAttribute(
           string tag)
        {
            _tag = tag;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var hasPermissions = await Security.IsValidateRequestAsync(context.HttpContext.Session, _tag);

                if (hasPermissions)
                {
                    await next();
                    return;
                }

                context.Result = new BadRequestObjectResult(new ProblemDetails()
                {
                    Status = (int)System.Net.HttpStatusCode.Unauthorized,
                    Title = "Unauthorized",
                })
                {
                    StatusCode = (int)System.Net.HttpStatusCode.Unauthorized
                };
            }
            catch (Exception)
            {
                context.Result = new BadRequestObjectResult(new ProblemDetails()
                {
                    Status = (int)System.Net.HttpStatusCode.InternalServerError,
                    Title = "Internal server error"
                })
                {
                    StatusCode = (int)System.Net.HttpStatusCode.InternalServerError,
                    Value = "Internal server error"
                };
            }
        }

    }
}
