using Authorization.API.Models;
using Shared.Models;

namespace Authorization.API.Services;

public interface IAuthorizationManager
{
    public Task<OperationResult> AuthenticateAsync(LoginRequest request);
    public Task<OperationResult> RegisterAsync(RegisterRequest request);
}
