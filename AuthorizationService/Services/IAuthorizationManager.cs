using AuthorizationService.Models;
using Shared.Models;

namespace AuthorizationService.Services;

public interface IAuthorizationManager
{
    public Task<OperationResult> AuthenticateAsync(LoginRequest request);
    public Task<OperationResult> RegisterAsync(RegisterRequest request);
    public Task<OperationResult> CreateRestorePasswordRequestAsync(RequestRestorePasswordRequest request);
    public Task<OperationResult> RestorePasswordAsync(RestorePasswordRequest request);
    public Task<OperationResult> ConfirmEmailAsync(string code);
    public Task<OperationResult> CreateConfirmEmailRequestAsync(RequestEmailConfirmRequest request);
}
