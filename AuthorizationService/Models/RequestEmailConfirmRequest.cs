using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public record RequestEmailConfirmRequest([EmailAddress] string Email);