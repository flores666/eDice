using System.ComponentModel.DataAnnotations;

namespace MailService.Models;

public record EmailRequest(
    [Required, EmailAddress]
    string To,
    [Required]
    string Subject,
    [Required]
    string Body
    );