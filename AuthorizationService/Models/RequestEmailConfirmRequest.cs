using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Models;

public class RequestEmailConfirmRequest
{ 
    [EmailAddress] 
    public string Email { get; set; }
}
