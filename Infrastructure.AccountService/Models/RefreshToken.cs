namespace Infrastructure.AccountService.Models;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedByIp { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? RevokedByIp { get; set; }

    public Guid? ReplacedByToken { get; set; }

    public virtual ICollection<RefreshToken> InverseReplacedByTokenNavigation { get; set; } = new List<RefreshToken>();

    public virtual RefreshToken? ReplacedByTokenNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
