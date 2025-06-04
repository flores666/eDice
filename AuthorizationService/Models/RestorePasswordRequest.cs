using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public record RestorePasswordRequest(
    [Required]
    [Length(8, 35)]
    string Password,
    string? Code = null);