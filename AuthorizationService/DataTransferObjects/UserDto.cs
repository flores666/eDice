namespace AuthorizationService.DataTransferObjects;

public class UserDto
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string PasswordHash { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordResetCode { get; set; }

    public short FailedLoginCount { get; set; }

    public DateTime? DisabledBefore { get; set; }
}
