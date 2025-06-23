namespace Infrastructure.AssetCrafterService.Models;

public partial class Token
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid Type { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsPublic { get; set; }

    public bool IsConfirmed { get; set; }

    public bool IsOfficial { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual TokenType TypeNavigation { get; set; } = null!;

    public virtual ICollection<Pack> Packs { get; set; } = new List<Pack>();
}
