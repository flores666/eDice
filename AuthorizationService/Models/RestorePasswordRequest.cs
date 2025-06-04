using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public record RestorePasswordRequest(
    [Required] 
    string Password,
    string? Code = null);