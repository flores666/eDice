using AuthorizationService.Models;
using AuthorizationService.Repository;
using Infrastructure.AuthorizationService.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Models;

namespace AuthorizationService.Services;

public class AuthorizationManager : IAuthorizationManager
{
    private readonly IUsersRepository _usersRepository;

    public AuthorizationManager(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<OperationResult> AuthenticateAsync(LoginRequest request)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserAsync(request.Login);
        if (user == null || new PasswordHasher<object>().VerifyHashedPassword(null, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            response.ErrorMessage = "Похоже, что пользователь с такими данными не существует";
            return response;
        }

        if (DateTime.UtcNow < user.DisabledBefore)
        {
            response.ErrorMessage = "Доступ к аккаунту ограничен. Попробуйте позже";
            return response;
        }
        
        return response;
    }

    public async Task<OperationResult> RegisterAsync(RegisterRequest request)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserAsync(request.Login);
        if (user != null)
        {
            response.ErrorMessage = user.EmailConfirmed 
                ? "Похоже, эта электронная почта уже зарегистрирована. Попробуйте войти или использовать другую" 
                : "Пожалуйста, подтвердите свою почту — мы отправили вам письмо с ссылкой";

            return response;
        }

        response.IsSuccess = await _usersRepository.CreateUserAsync(new User
        {
            Email = request.Login,
            Name = request.UserName,
            PasswordHash = new PasswordHasher<object>().HashPassword(null, request.Password)
        });

        return response;
    }
}
