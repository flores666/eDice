using Microsoft.AspNetCore.Mvc;

namespace AssetCrafterService.Models;

public class FilterModel
{
    [FromQuery(Name = "size")]
    public int Size { get; set; } = 20;
    
    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;
    
    [FromQuery(Name = "search")]
    public string? Search { get; set; }

    [FromQuery(Name = "sort")]
    public TokensSortOption Sort { get; set; } = TokensSortOption.NewFirst;

    [FromQuery(Name = "officialOnly")]
    public bool? OfficialOnly { get; set; }

    [FromQuery(Name = "confirmedOnly")]
    public bool? ConfirmedOnly { get; set; }
    
    [FromQuery(Name = "public")]
    public bool? PublicOnly { get; set; }
    
    [FromQuery(Name = "type")]
    public Guid? TokenType { get; set; }
}