﻿using AuthorizationService.Configuration;
using AuthorizationService.Helpers;
using AuthorizationService.Helpers.UserValidator;
using AuthorizationService.Models;
using AuthorizationService.Models.Tokens;
using AuthorizationService.Repository;
using Infrastructure.AuthorizationService.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Lib.Extensions;
using Shared.MessageBus.Kafka.Producer;
using Shared.Models;

namespace AuthorizationService.Services;

public class AuthorizationManager : IAuthorizationManager
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokensRepository _tokensRepository;
    private readonly IMessagesProducer<EmailMessageEvent> _messagesProducer;

    public AuthorizationManager(IUsersRepository usersRepository, ITokensRepository tokensRepository,
        IMessagesProducer<EmailMessageEvent> messagesProducer)
    {
        _usersRepository = usersRepository;
        _tokensRepository = tokensRepository;
        _messagesProducer = messagesProducer;
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

        var token = TokenGenerator.CreateTokens(user, request.UserIp);
        
        response.IsSuccess = await _tokensRepository.CreateTokenAsync(token.RefreshToken);
        if (response.IsSuccess)
        {
            response.Data = new TokenResultModel
            {
                RefreshToken = token.RefreshToken.Token,
                AccessToken = token.AccessToken
            };
        }

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

        var confirmCode = Guid.NewGuid().ToString().AsSpan(0, 8).ToString();
        response.IsSuccess = await _usersRepository.CreateUserAsync(new User
        {
            Email = request.Login,
            Name = request.UserName,
            PasswordHash = new PasswordHasher<object>().HashPassword(null, request.Password),
            EmailConfirmed = false,
            CodeRequestedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            ResetCode = confirmCode
        });

        if (response.IsSuccess)
        {
            await _messagesProducer.PublishAsync(KafkaTopics.UserCreated, GetConfirmEmailModel(request.Login, confirmCode, request.ReturnUrl));
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
            var nextCodeRequestDate = user.CodeRequestedAt.Value.Add(Constants.RestoreCodeDelay);
            if (DateTime.UtcNow < nextCodeRequestDate)
            {
                var time = (nextCodeRequestDate - DateTime.UtcNow).ToReadableString();
                response.Message = $"Запросить код повторно можно будет через {time}";
                return response;
            }
        }

        user.ResetCode = Guid.NewGuid().ToString().AsSpan(0, 8).ToString();
        user.CodeRequestedAt = DateTime.UtcNow;

        response.IsSuccess = await _usersRepository.UpdateUserAsync(user);
        if (response.IsSuccess)
        {
            await _messagesProducer.PublishAsync(KafkaTopics.PasswordResetRequested, GetRestorePasswordEmailModel(user.Email, user.ResetCode, request.ReturnUrl));
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
        if (!UserValidator.IsValidWithEmail(user, out var message) ||
            !UserValidator.IsRestoreCodeValid(user, out message))
        {
            response.Message = message;
            return response;
        }

        user.PasswordHash = new PasswordHasher<object>().HashPassword(null, request.Password);
        user.CodeRequestedAt = null;
        user.ResetCode = null;
        response.IsSuccess = await _usersRepository.UpdateUserAsync(user);

        return response;
    }

    public async Task<OperationResult> ConfirmEmailAsync(string code)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserByRestoreCodeAsync(code);
        if (!UserValidator.IsValidWithEmail(user, out var message) ||
            !UserValidator.IsRestoreCodeValid(user, out message))
        {
            response.Message = message;
            return response;
        }

        user.EmailConfirmed = true;
        user.CodeRequestedAt = null;
        user.ResetCode = null;
        response.IsSuccess = await _usersRepository.UpdateUserAsync(user);

        return response;
    }

    public async Task<OperationResult> CreateConfirmEmailRequestAsync(RequestEmailConfirmRequest request)
    {
        var response = new OperationResult();

        var user = await _usersRepository.GetUserByLoginAsync(request.Email);
        if (!UserValidator.IsValid(user, out var message))
        {
            response.Message = message;
            return response;
        }

        if (user.CodeRequestedAt != null)
        {
            var nextCodeRequestDate = user.CodeRequestedAt.Value.Add(Constants.ConfirmEmailDelay);
            if (DateTime.UtcNow < nextCodeRequestDate)
            {
                var time = (nextCodeRequestDate - DateTime.UtcNow).ToReadableString();
                response.Message = $"Запросить код повторно можно будет через {time}";
                return response;
            }
        }
        
        user.ResetCode = Guid.NewGuid().ToString().AsSpan(0, 8).ToString();
        user.CodeRequestedAt = DateTime.UtcNow;

        response.IsSuccess = await _usersRepository.UpdateUserAsync(user);
        if (response.IsSuccess)
        {
            await _messagesProducer.PublishAsync(KafkaTopics.EmailConfirmRequested, GetConfirmEmailModel(request.Email, user.ResetCode, request.ReturnUrl));
        }

        return response;
    }
    
    public async Task<OperationResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var response = new OperationResult();

        var refreshToken = await _tokensRepository.GetByRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            response.Message = "Недействительный токен обновления";
            return response;
        }

        var user = await _usersRepository.GetUserByIdAsync(refreshToken.UserId);
        if (user == null)
        {
            response.Message = "Пользователь не найден";
            return response;
        }

        var newRefreshToken = TokenGenerator.GenerateRefreshToken(user.Id, request.Ip);
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = request.Ip;
        refreshToken.ReplacedByToken = newRefreshToken.Id;

        response.IsSuccess = await SaveRefreshTokensAsync(refreshToken, newRefreshToken);
        if (response.IsSuccess) response.Data = TokenGenerator.CreateTokens(user, request.Ip);

        return response;
    }

    public async Task<OperationResult> LogoutAsync(RefreshTokenRequest request)
    {
        var response = new OperationResult();

        var refreshToken = await _tokensRepository.GetByRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            response.Message = "Недействительный токен обновления";
            return response;
        }
        
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = request.Ip;
        refreshToken.ExpiresAt = DateTime.UtcNow;
        
        response.IsSuccess = await _tokensRepository.UpdateAsync(refreshToken);
        
        return response;
    }

    private async Task<bool> SaveRefreshTokensAsync(RefreshToken tokenToPatch, RefreshToken newToken)
    {
        await using var transaction = await _tokensRepository.BeginTransactionAsync();

        try
        {
            await _tokensRepository.UpdateAsync(tokenToPatch);
            await _tokensRepository.CreateTokenAsync(newToken);
        }
        catch (Exception e)
        {
            //todo log
            return false;
        }

        await transaction.CommitAsync();
        
        return true;
    }

    private static EmailMessageEvent GetConfirmEmailModel(string email, string code, string returnUrl)
    {
        return new EmailMessageEvent
        {
            To = email,
            Subject = "Подтверждение адреса электронной почты",
            Body = EmailTemplates.GetConfirmEmailBody(returnUrl, code)
        };
    }

    private static EmailMessageEvent GetRestorePasswordEmailModel(string email, string code, string returnUrl)
    {
        return new EmailMessageEvent
        {
            To = email,
            Subject = "Восстановление пароля",
            Body = EmailTemplates.GetRestorePasswordBody(returnUrl, code)
        };
    }
}