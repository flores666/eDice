using AssetCrafterService.Models;
using Shared.Models;

namespace AssetCrafterService.Services;

public interface ITokensService
{
    public Task<List<TokenDto>> GetTokensAsync(FilterModel filter);
    public Task<OperationResult> CreateTokenAsync(TokenDto token);
    public Task<OperationResult> UpdateTokenAsync(TokenDto token);
    public Task<OperationResult> DeleteTokenAsync(Guid id);
}