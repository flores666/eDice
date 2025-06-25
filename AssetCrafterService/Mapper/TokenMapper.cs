using AssetCrafterService.Models;
using Infrastructure.AssetCrafterService.Models;

namespace AssetCrafterService.Mapper;

public static class TokenMapper
{
    public static TokenDto ToDto(Token model)
    {
        return new TokenDto
        {
            Description = model.Description,
            CreatedBy = model.CreatedBy,
            Id = model.Id,
            Name = model.Name,
            Type = model.Type,
            ImageUrl = model.ImageUrl,
            IsPublic = model.IsPublic,
            IsOfficial = model.IsOfficial,
            IsConfirmed = model.IsConfirmed
        };
    }
    
    public static Token ToEntity(TokenDto model)
    {
        return new Token
        {
            Description = model.Description,
            CreatedBy = model.CreatedBy,
            Id = model.Id,
            Name = model.Name,
            Type = model.Type,
            ImageUrl = model.ImageUrl,
            IsPublic = model.IsPublic,
            IsOfficial = model.IsOfficial,
            IsConfirmed = model.IsConfirmed
        };
    }
}