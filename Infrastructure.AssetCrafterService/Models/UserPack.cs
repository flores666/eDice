namespace Infrastructure.AssetCrafterService.Models;

/// <summary>
/// Связи пользователей с паками
/// </summary>
public partial class UserPack
{
    public Guid UserId { get; set; }

    public Guid PackId { get; set; }

    public DateTime GrantedAt { get; set; }

    public virtual Pack Pack { get; set; } = null!;
}
