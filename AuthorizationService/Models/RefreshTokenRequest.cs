using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AuthorizationService.Models;

public record RefreshTokenRequest(
    [Required] string RefreshToken,
    [ValidateNever] string? Ip);