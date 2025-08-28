using System.ComponentModel.DataAnnotations;

namespace AccountService.Models;

public class RequestRestorePasswordRequest
{
    [Required(ErrorMessage = "Поле «Логин» обязательно для заполнения.")]
    [EmailAddress(ErrorMessage = "Поле «Логин» должно содержать корректный email.")]
    [Length(2, 35, ErrorMessage = "Поле «Логин» должно содержать от {1} до {2} символов.")]
    public string Login { get; set; } = default!;
}