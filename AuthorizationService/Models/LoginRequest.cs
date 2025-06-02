using System.ComponentModel.DataAnnotations;

namespace Authorization.API.Models;

public record LoginRequest(
    [Required]
    [EmailAddress]
    [Length(2, 35)]
    string Login,
    [Required] [Length(8, 35)] string Password);