using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class RegisterRequest
{
    [Required(ErrorMessage = "Поле «Логин» обязательно для заполнения.")]
    [EmailAddress(ErrorMessage = "Поле «Логин» должно содержать корректный email.")]
    [Length(2, 35, ErrorMessage = "Поле «Логин» должно содержать от {1} до {2} символов.")]
    public string Login { get; set; } = default!;

    [Required(ErrorMessage = "Поле «Имя пользователя» обязательно для заполнения.")]
    [Length(2, 35, ErrorMessage = "Поле «Имя пользователя» должно содержать от {1} до {2} символов.")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "Поле «Пароль» обязательно для заполнения.")]
    [Length(2, 35, ErrorMessage = "Поле «Пароль» должно содержать от {1} до {2} символов.")]
    public string Password { get; set; } = default!;
}