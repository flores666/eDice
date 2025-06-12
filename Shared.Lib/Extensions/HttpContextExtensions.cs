using Microsoft.AspNetCore.Http;

namespace Shared.Lib.Extensions;

public static class HttpContextExtensions
{
    public static string? GetUserIp(this HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor)) return forwardedFor.Split(',').FirstOrDefault()?.Trim();
        return context.Connection.RemoteIpAddress?.ToString();
    }
}