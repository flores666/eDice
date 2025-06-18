using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.Lib.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DenyAuthenticatedAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            context.Result = new ForbidResult();
        }
    }
}