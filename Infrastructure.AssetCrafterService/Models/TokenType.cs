namespace Infrastructure.AssetCrafterService.Models;

public partial class TokenType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Caption { get; set; } = null!;

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
