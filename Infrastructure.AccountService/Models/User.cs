namespace Infrastructure.AccountService.Models;

public partial class User
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public string? ResetCode { get; set; }

    public DateTime? CodeRequestedAt { get; set; }

    public DateTime? BannedBefore { get; set; }
    
    public string? Description { get; set; }

    public string? ProfilePicture { get; set; }

    public string? ProfilePicturePreview { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
