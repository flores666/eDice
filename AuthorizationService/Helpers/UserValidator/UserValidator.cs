using System.Diagnostics.CodeAnalysis;
using AuthorizationService.Configuration;
using Infrastructure.AuthorizationService.Models;

namespace AuthorizationService.Helpers.UserValidator;

public static class UserValidator
{
    public static bool IsValid([NotNullWhen(true)] User? user, out string errorMessage)
    {
        if (user == null)
        {
            errorMessage = "Похоже, что пользователь с такими данными не существует";
            return false;
        }

        if (DateTime.UtcNow < user.BannedBefore)
        {
            errorMessage = "Доступ к аккаунту ограничен. Попробуйте позже";
            return false;
        }

        errorMessage = "";
        return true;
    }

    public static bool IsValidWithEmail([NotNullWhen(true)] User? user, out string errorMessage)
    {
        var isValid = IsValid(user, out errorMessage);
        if (isValid)
        {
            if (!user.EmailConfirmed)
            {
                errorMessage = "Для начала нужно подтвердить адрес электронной почты";
                return false;
            }
        }

        errorMessage = "";
        return true;
    }

    public static bool IsRestoreCodeValid([NotNullWhen(true)] User? user, out string errorMessage)
    {
        var isValid = IsValid(user, out errorMessage);
        if (isValid)
        {
            if (user.CodeRequestedAt != null && DateTime.UtcNow > user.CodeRequestedAt.Value.Add(Constants.RestoreCodeTimeAlive))
            {
                errorMessage = "К сожалению, срок действия кода истёк. Пожалуйста, отправьте запрос ещё раз, чтобы получить новый код";
                return false;
            }
        }

        return true;
    }
}