namespace Infrastructure.AssetCrafterService.Models;

/// <summary>
/// Паки токенов
/// </summary>
public partial class Pack
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsPublic { get; set; }

    public bool IsConfirmed { get; set; }

    public bool IsOfficial { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserPack> UserPacks { get; set; } = new List<UserPack>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
