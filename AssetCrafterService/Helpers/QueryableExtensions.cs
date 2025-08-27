using AssetCrafterService.Models;
using Infrastructure.AssetCrafterService.Models;

namespace AssetCrafterService.Helpers;

public static class QueryableExtensions
{
    public static IOrderedQueryable<Token> OrderTokens(this IQueryable<Token> query, TokensSortOption sortOption) =>
        sortOption switch
        {
            TokensSortOption.Asc => query.OrderBy(w => w.Name),
            TokensSortOption.Desc => query.OrderByDescending(w => w.Name),
            TokensSortOption.Confirmed => query.OrderByDescending(w => w.IsConfirmed),
            TokensSortOption.Official => query.OrderByDescending(w => w.IsOfficial),
            TokensSortOption.NewFirst => query.OrderBy(w => w.CreatedAt),
            TokensSortOption.OldFirst => query.OrderByDescending(w => w.CreatedAt),
            _ =>  query.OrderByDescending(w => w.CreatedAt)
        };
}