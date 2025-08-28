using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AccountService.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "Поле «Логин» обязательно для заполнения.")]
    [EmailAddress(ErrorMessage = "Поле «Логин» должно содержать корректный email.")]
    [Length(2, 35, ErrorMessage = "Поле «Логин» должно содержать от {1} до {2} символов.")]
    public string Login { get; set; } = default!;

    [Required(ErrorMessage = "Поле «Пароль» обязательно для заполнения.")]
    [Length(8, 35, ErrorMessage = "Поле «Пароль» должно содержать от {1} до {2} символов.")]
    public string Password { get; set; } = default!;

    [ValidateNever]
    public string? UserIp { get; set; }
}
