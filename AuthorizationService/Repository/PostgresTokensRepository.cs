using Infrastructure.AuthorizationService;
using Infrastructure.AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthorizationService.Repository;

public class PostgresTokensRepository : ITokensRepository
{
    private readonly PostgresContext _context;

    public PostgresTokensRepository(PostgresContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(w => w.Token == token);
    }

    public async Task<bool> CreateTokenAsync(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(RefreshToken token)
    {
        if (token.Id == Guid.Empty) return false;

        var trackedUser = _context.ChangeTracker.Entries<RefreshToken>().FirstOrDefault(w => w.Entity.Id == token.Id);
        if (trackedUser != null)
        {
            trackedUser.CurrentValues.SetValues(token);
        }
        else
        {
            _context.RefreshTokens.Attach(token);
            _context.Entry(token).State = EntityState.Modified;
        }

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
