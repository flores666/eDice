using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class RestorePasswordRequest
{
    [Required] [Length(8, 35)] 
    public string Password { get; set; }
    public string? Code { get; set; } = null;
}