using Authorization.API.Models;
using Shared.Models;

namespace Authorization.API.Services;

public class AuthorizationManager : IAuthorizationManager
{
    public async Task<OperationResult> AuthenticateAsync(LoginRequest request)
    {
        var response = new OperationResult
        {
            ErrorMessage = "Пока что не реализовано, тестируем api"
        };
        
        return response;
    }

    public async Task<OperationResult> RegisterAsync(RegisterRequest request)
    {
        var response = new OperationResult
        {
            ErrorMessage = "Пока что не реализовано, тестируем api"
        };
        
        return response;
    }
}