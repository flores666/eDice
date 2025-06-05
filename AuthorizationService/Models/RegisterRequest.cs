using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class RegisterRequest
{
    [Required, EmailAddress, MinLength(2), MaxLength(35)]
    public string Login { get; set; } = default!;

    [Required, MinLength(2), MaxLength(35)]
    public string UserName { get; set; } = default!;

    [Required, MinLength(2), MaxLength(35)]
    public string Password { get; set; } = default!;
}
