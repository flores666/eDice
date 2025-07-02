using AssetCrafterService.Mapper;
using AssetCrafterService.Models;
using Infrastructure.AssetCrafterService;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace AssetCrafterService.Services;

public class TokensService : ITokensService
{
    private readonly PostgresContext _context;

    public TokensService(PostgresContext context)
    {
        _context = context;
    }
    
    public async Task<List<TokenDto>> GetTokensAsync(FilterModel filter)
    {
        return await _context.Tokens
            .OrderBy(o => o.CreatedAt)
            .Paginate(filter.Page, filter.Size)
            .Select(s => TokenMapper.ToDto(s))
            .ToListAsync();
    }

    public async Task<TokenDto?> GetTokenAsync(Guid id)
    {
        var token = await _context.Tokens.FindAsync(id);
        if (token == null) return null;
        
        return TokenMapper.ToDto(token);
    }

    public async Task<OperationResult> CreateTokenAsync(TokenDto token)
    {
        if (token.Id == Guid.Empty) token.Id = Guid.NewGuid();

        _context.Tokens.Add(TokenMapper.ToEntity(token));
        var isSuccess = await _context.SaveChangesAsync() > 0;
        if (isSuccess) return OperationResult.Ok(); 
                
        return OperationResult.Fail("Что-то пошло не так, изменения сохранить не удалось");
    }

    public async Task<OperationResult> UpdateTokenAsync(TokenDto tokenDto)
    {
        var token = await _context.Tokens.FindAsync(tokenDto.Id);
        if (token == null) return OperationResult.Fail("Токен не найден");
        
        var isChanged = TokenMapper.ChangeEntity(tokenDto, token);
        if (!isChanged) return OperationResult.Ok("Ничего не изменилось");
        
        var isSuccess = await _context.SaveChangesAsync() > 0;
        if (isSuccess) return OperationResult.Ok();
        
        return OperationResult.Fail("Что-то пошло не так, изменения сохранить не удалось");
    }

    public async Task<OperationResult> DeleteTokenAsync(Guid id)
    {
        var token = await _context.Tokens.FindAsync(id);
        if (token == null) return OperationResult.Fail("Такой токен не найден");
        
        _context.Tokens.Remove(token);
        var isSuccess = await _context.SaveChangesAsync() > 0;
        if (isSuccess) return OperationResult.Ok();
        
        return OperationResult.Fail("Что-то пошло не так, изменения сохранить не удалось");
    }
}