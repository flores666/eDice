using AccountService.Models;
using Shared.Models;

namespace AccountService.Services;

public interface IAuthorizationManager
{
    public Task<OperationResult> AuthenticateAsync(LoginRequest request);
    public Task<OperationResult> RegisterAsync(RegisterRequest request);
    public Task<OperationResult> CreateRestorePasswordRequestAsync(RequestRestorePasswordRequest request);
    public Task<OperationResult> RestorePasswordAsync(RestorePasswordRequest request);
    public Task<OperationResult> ConfirmEmailAsync(string code);
    public Task<OperationResult> CreateConfirmEmailRequestAsync(RequestEmailConfirmRequest request);
    public Task<OperationResult> RefreshTokenAsync(RefreshTokenRequest request);
    public Task<OperationResult> LogoutAsync(RefreshTokenRequest request);
}
