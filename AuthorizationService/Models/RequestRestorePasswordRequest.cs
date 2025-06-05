using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class RequestRestorePasswordRequest
{
    [Required]
    [EmailAddress]
    [Length(2, 35)]
    public string Login { get; set; }
}