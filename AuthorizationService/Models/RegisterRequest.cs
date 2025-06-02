using System.ComponentModel.DataAnnotations;

namespace Authorization.API.Models;

public record RegisterRequest(
    [Required]
    [EmailAddress]
    [Length(2, 35)]
    string Login,
    [Required] [Length(2, 35)] string UserName,
    [Required] [Length(8, 35)] string Password);