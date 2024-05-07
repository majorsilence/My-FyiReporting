using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace fyiReporting.ReportServerMvc.Controllers
{
    public class HasReportPermissionsAttribute : IAsyncActionFilter
    {
        private readonly ILogger<HasReportPermissionsAttribute> _logger;

        public HasReportPermissionsAttribute(
            ILogger<HasReportPermissionsAttribute> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                string? sessionValue = context.HttpContext.Session.GetString("CurrentPdfReport");
                if (string.IsNullOrEmpty(sessionValue))
                {
                    context.Result = new BadRequestObjectResult(new ProblemDetails()
                    {
                        Status = (int)System.Net.HttpStatusCode.BadRequest,
                        Title = "Bad request",
                    })
                    {
                        StatusCode = (int)System.Net.HttpStatusCode.BadRequest
                    };
                    return;
                }
                ReportSession ses = System.Text.Json.JsonSerializer.Deserialize<ReportSession>(sessionValue);

                if (Security.HasPermissions(ses.url))
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
            catch (Exception ex)
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
                _logger.LogError(ex, "Error validating request");
            }
        }

    }
}
