using Microsoft.AspNetCore.Http;
using Shared.Logging;

namespace Shared.Lib.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAppLogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, IAppLogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled error occurred");
            context.Response.StatusCode = 500;
        }
    }
}
