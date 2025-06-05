using AuthorizationService.Configuration;
using AuthorizationService.Helpers;
using AuthorizationService.Helpers.UserValidator;
using AuthorizationService.Models;
using AuthorizationService.Repository;
using Infrastructure.AuthorizationService.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Lib.Extensions;
using Shared.Models;

namespace AuthorizationService.Services;

public class AuthorizationManager : IAuthorizationManager
{
    private readonly IUsersRepository _usersRepository;
    private readonly HttpContext _httpContext;

    public AuthorizationManager(IUsersRepository usersRepository, IHttpContextAccessor httpContextAccessor)
    {
        _usersRepository = usersRepository;
        _httpContext = httpContextAccessor.HttpContext ?? new DefaultHttpContext();
    }

    public async Task<OperationResult> AuthenticateAsync(LoginRequest request)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserByLoginAsync(request.Login);
        if (user == null || new PasswordHasher<object>().VerifyHashedPassword(null, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        {
            response.Message = "Похоже, что пользователь с такими данными не существует";
            return response;
        }

        if (!UserValidator.IsValidWithEmail(user, out var message))
        {
            response.Message = message;
            if (!user.EmailConfirmed) response.Data = user.Email;
            
            return response;
        }
        
        response.IsSuccess = true;
        response.Data = JwtTokenGenerator.GenerateJwtToken(user);

        return response;
    }

    public async Task<OperationResult> RegisterAsync(RegisterRequest request)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserByLoginAsync(request.Login);
        if (user != null)
        {
            response.Message = user.EmailConfirmed
                ? "Похоже, эта электронная почта уже зарегистрирована. Попробуйте войти или использовать другую"
                : "Пожалуйста, подтвердите свою почту — мы отправили вам письмо с ссылкой";

            return response;
        }

        response.IsSuccess = await _usersRepository.CreateUserAsync(new User
        {
            Email = request.Login,
            Name = request.UserName,
            PasswordHash = new PasswordHasher<object>().HashPassword(null, request.Password),
            EmailConfirmed = false,
            CodeRequestedAt = DateTime.UtcNow
        });

        if (response.IsSuccess)
        {
            //todo publish event
        }

        return response;
    }

    public async Task<OperationResult> CreateRestorePasswordRequestAsync(RequestRestorePasswordRequest request)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserByLoginAsync(request.Login);
        if (!UserValidator.IsValidWithEmail(user, out var message))
        {
            response.Message = message;
            return response;
        }

        if (user.CodeRequestedAt != null)
        {
            var nextCodeRequestDate = user.CodeRequestedAt.Value.Add(Constants.RestoreCodeTimeAlive);
            if (DateTime.UtcNow < nextCodeRequestDate)
            {
                var time = (nextCodeRequestDate - DateTime.UtcNow).ToReadableString();
                response.Message = $"Запросить код повторно можно будет через {time}";
                return response;
            }
        }

        user.ResetCode = Guid.NewGuid().ToString().AsSpan(0, 8).ToString();
        user.CodeRequestedAt = DateTime.UtcNow;
        
        var isUpdated = await _usersRepository.UpdateUserAsync(user);
        if (isUpdated)
        {
            var restoreLink = _httpContext.Request.Host.ToUriComponent().TrimEnd('/') + "/auth/restore/" + user.ResetCode;
            //todo publish event
            response.IsSuccess = true;
            response.Message = $"Пока сообщения не работают, вот вам ссылка на восстановление {restoreLink}";
        }

        return response;
    }

    public async Task<OperationResult> RestorePasswordAsync(RestorePasswordRequest request)
    {
        var response = new OperationResult();
        if (string.IsNullOrEmpty(request.Code))
        {
            response.Message = "Код для восстановления пароля не найден";
            return response;
        }
        
        var user = await _usersRepository.GetUserByRestoreCodeAsync(request.Code);
        if (!UserValidator.IsValidWithEmail(user, out var message) || !UserValidator.IsRestoreCodeValid(user, out message))
        {
            response.Message = message;
            return response;
        }

        user.PasswordHash = new PasswordHasher<object>().HashPassword(null, request.Password);
        response.IsSuccess = await _usersRepository.UpdateUserAsync(user);

        return response;
    }

    public async Task<OperationResult> ConfirmPasswordAsync(string code)
    {
        var response = new OperationResult();
        
        var user = await _usersRepository.GetUserByRestoreCodeAsync(code);
        if (!UserValidator.IsValidWithEmail(user, out var message) || !UserValidator.IsRestoreCodeValid(user, out message))
        {
            response.Message = message;
            return response;
        }

        user.EmailConfirmed = true;
        response.IsSuccess = await _usersRepository.UpdateUserAsync(user);
        
        return response;
    }

    public async Task<OperationResult> CreateConfirmPasswordRequestAsync(string email)
    {
        var response = new OperationResult();
        
        var user = await _usersRepository.GetUserByLoginAsync(email);
        if (!UserValidator.IsValid(user, out var message))
        {
            response.Message = message;
            return response;
        }
        
        user.ResetCode = Guid.NewGuid().ToString().AsSpan(0, 8).ToString();
        user.CodeRequestedAt = DateTime.UtcNow;

        response.IsSuccess = true;
        
        return response;
    }
}
