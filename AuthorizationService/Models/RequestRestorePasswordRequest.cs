using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public record RequestRestorePasswordRequest(
    [Required]
    [EmailAddress]
    [Length(2, 35)]
    string Login);