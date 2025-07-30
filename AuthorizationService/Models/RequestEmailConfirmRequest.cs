using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class RequestEmailConfirmRequest
{
    [EmailAddress(ErrorMessage = "Поле «Email» должно содержать корректный email.")]
    public string Email { get; set; } = default!;
}