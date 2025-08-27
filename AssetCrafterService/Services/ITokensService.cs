using AssetCrafterService.Models;
using Shared.Models;

namespace AssetCrafterService.Services;

public interface ITokensService
{
    public Task<PaginatedList<TokenDto>> GetTokensAsync(FilterModel filter);
    public Task<TokenDto?> GetTokenAsync(Guid id);
    public Task<OperationResult> CreateTokenAsync(TokenDto token);
    public Task<OperationResult> UpdateTokenAsync(TokenDto token);
    public Task<OperationResult> DeleteTokenAsync(Guid id);
}