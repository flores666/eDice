using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class RestorePasswordRequest
{
    [Required(ErrorMessage = "Поле «Пароль» обязательно для заполнения.")]
    [Length(8, 35, ErrorMessage = "Поле «Пароль» должно содержать от {1} до {2} символов.")]
    public string Password { get; set; } = default!;

    public string? Code { get; set; } = null;
}