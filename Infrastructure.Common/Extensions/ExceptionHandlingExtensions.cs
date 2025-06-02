using Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Common.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
        return builder;
    }
}
