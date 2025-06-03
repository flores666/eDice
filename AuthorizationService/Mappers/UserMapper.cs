using AuthorizationService.DataTransferObjects;
using Infrastructure.AuthorizationService.Models;

namespace AuthorizationService.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User model)
    {
        return new UserDto
        {
            Id = model.Id,
            Email = model.Email,
            Name = model.Name,
            CreatedAt = model.CreatedAt,
            DisabledBefore = model.DisabledBefore,
            EmailConfirmed = model.EmailConfirmed,
            PasswordHash = model.PasswordHash,
            FailedLoginCount = model.FailedLoginCount,
            PasswordResetCode = model.PasswordResetCode
        };
    }
    
    public static User ToModel(UserDto model)
    {
        return new User
        {
            Id = model.Id,
            Email = model.Email,
            Name = model.Name,
            CreatedAt = model.CreatedAt,
            DisabledBefore = model.DisabledBefore,
            EmailConfirmed = model.EmailConfirmed,
            PasswordHash = model.PasswordHash,
            FailedLoginCount = model.FailedLoginCount,
            PasswordResetCode = model.PasswordResetCode
        };
    }
}