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

    public static bool ChangeEntity(TokenDto model, Token entity)
    {
        var hasChanges = false;

        if (model.Description != entity.Description)
        {
            entity.Description = model.Description;
            hasChanges = true;
        }
        
        if (model.Name != entity.Name)
        {
            entity.Name = model.Name;
            hasChanges = true;
        }

        if (model.ImageUrl != entity.ImageUrl)
        {
            entity.ImageUrl = model.ImageUrl;
            hasChanges = true;
        }

        if (model.IsPublic != entity.IsPublic)
        {
            entity.IsPublic = model.IsPublic;
            hasChanges = true;
        }

        if (model.IsOfficial != entity.IsOfficial)
        {
            entity.IsOfficial = model.IsOfficial;
            hasChanges = true;
        }

        if (model.IsConfirmed != entity.IsConfirmed)
        {
            entity.IsConfirmed = model.IsConfirmed;
            hasChanges = true;
        }

        return hasChanges;
    }
}