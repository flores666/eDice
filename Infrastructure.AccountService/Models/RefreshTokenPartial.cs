namespace Infrastructure.AccountService.Models;

public partial class RefreshToken
{
    public bool IsActive => DateTime.UtcNow < ExpiresAt && RevokedAt == null && ReplacedByToken == null;
}
