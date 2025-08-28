using Infrastructure.AccountService.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountService.Repository;

public interface ITokensRepository
{
    public Task<RefreshToken?> GetByRefreshTokenAsync(string token);
    public Task<bool> CreateTokenAsync(RefreshToken token);
    public Task<bool> UpdateAsync(RefreshToken token);
    public Task<IDbContextTransaction> BeginTransactionAsync();
}
