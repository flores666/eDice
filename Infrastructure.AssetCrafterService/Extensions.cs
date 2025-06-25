using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AssetCrafterService;

public static class Extensions
{
    public static IQueryable<TEntity> Paginate<TEntity>(this DbSet<TEntity> dbSet, int page, int size)
        where TEntity : class => dbSet.Skip((page - 1) * size).Take(size);
}