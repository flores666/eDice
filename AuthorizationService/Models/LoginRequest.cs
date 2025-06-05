using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [Length(2, 35)]
    public string Login { get; set; }

    [Required]
    [Length(8, 35)]
    public string Password { get; set; }
}