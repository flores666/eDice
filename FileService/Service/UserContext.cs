using System.Security.Claims;

namespace FileService.Service;

public class UserContext : IUserContext
{
    public Guid Id { get; }

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false)
        {
            if (Guid.TryParse(
                    httpContextAccessor.HttpContext?.User.Claims
                        .FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier)?.Value, out var guid)) Id = guid;
        }
    }
}