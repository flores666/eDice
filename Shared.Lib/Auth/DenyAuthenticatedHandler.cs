using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Lib.Auth;

public class DenyAuthenticatedRequirement : IAuthorizationRequirement
{
}

public class DenyAuthenticatedHandler : AuthorizationHandler<DenyAuthenticatedRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        DenyAuthenticatedRequirement requirement)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public static class DenyAuthenticatedExtensions
{
    public static IServiceCollection AddDenyAuthenticatedHandler(this IServiceCollection collection)
    {
        collection.AddSingleton<IAuthorizationHandler, DenyAuthenticatedHandler>();
        return collection;
    }
}

public static class AuthorizationOptionsExtensions
{
    public static void AddDenyAuthenticatedPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy("DenyAuthenticated", policy =>
            policy.Requirements.Add(new DenyAuthenticatedRequirement()));
    }
}