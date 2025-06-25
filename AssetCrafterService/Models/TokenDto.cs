using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AssetCrafterService.Models;

public class TokenDto
{
    [ValidateNever]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; }

    [Required]
    public Guid Type { get; set; }

    public string? ImageUrl { get; set; }
    
    [Required]
    public bool IsPublic { get; set; }
    
    public Guid CreatedBy { get; set; }
    public bool IsOfficial { get; set; }
    public bool IsConfirmed { get; set; }
}
