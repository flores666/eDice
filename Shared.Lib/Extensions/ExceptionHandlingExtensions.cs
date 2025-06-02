using Microsoft.AspNetCore.Builder;
using Shared.Lib.Middleware;

namespace Shared.Lib.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
        
        return builder;
    }
}
