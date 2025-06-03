using AuthorizationService.Models;
using Shared.Models;

namespace AuthorizationService.Services;

public interface IAuthorizationManager
{
    public Task<OperationResult> AuthenticateAsync(LoginRequest request);
    public Task<OperationResult> RegisterAsync(RegisterRequest request);
}
