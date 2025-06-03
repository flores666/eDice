using AuthorizationService.DataTransferObjects;
using AuthorizationService.Mappers;
using AuthorizationService.Models;
using AuthorizationService.Repository;
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
        var response = new OperationResult
        {
            ErrorMessage = "Пока что не реализовано, тестируем api"
        };

        return response;
    }

    public async Task<OperationResult> RegisterAsync(RegisterRequest request)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserAsync(request.Login);
        if (user != null)
        {
            if (user.EmailConfirmed)
                response.ErrorMessage = "Похоже, эта электронная почта уже зарегистрирована. Попробуйте войти или использовать другую";
            else response.ErrorMessage = "Пожалуйста, подтвердите свою почту — мы отправили вам письмо с ссылкой";

            return response;
        }

        response = await _usersRepository.CreateUserAsync(UserMapper.ToModel(new UserDto
        {
            Email = request.Login,
            Name = request.UserName,
            PasswordHash = new PasswordHasher<object>().HashPassword(null, request.Password)
        }));

        return response;
    }
}
