using System.Security.Claims;
using AssetCrafterService.Models;
using AssetCrafterService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace AssetCrafterService.Controllers;

[Authorize]
[ApiController]
[Route("/tokens")]
public class TokensController : Controller
{
    private readonly ITokensService _tokensService;

    public TokensController(ITokensService tokensService)
    {
        _tokensService = tokensService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<OperationResult> GetTokens([FromQuery] FilterModel filter)
    {
        var result = new OperationResult
        {
            Data = await _tokensService.GetTokensAsync(filter)
        };

        return result;
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<OperationResult> GetToken(Guid id)
    {
        var result = new OperationResult
        {
            Data = await _tokensService.GetTokenAsync(id)
        };

        return result;
    }

    [HttpPost]
    public async Task<OperationResult> CreateToken(TokenDto token)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;

        if (ModelState.IsValid)
        {
            if (Guid.TryParse(User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                token.CreatedBy = userId;
                
                var result = await _tokensService.CreateTokenAsync(token);
                if (!result.IsSuccess) Response.StatusCode = StatusCodes.Status500InternalServerError;
                
                return result;
            }

            return OperationResult.Fail("Идентификатор пользователя не найден, вы точно авторизованы?");
        }

        return OperationResult.Fail("Пожалуйста, проверьте, всё ли верно заполнено.");
    }

    [HttpPatch]
    public async Task<OperationResult> UpdateToken(TokenDto token)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;

        if (ModelState.IsValid)
        {
            if (Guid.TryParse(User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                token.CreatedBy = userId;
                
                var result = await _tokensService.UpdateTokenAsync(token);
                if (!result.IsSuccess) Response.StatusCode = StatusCodes.Status500InternalServerError;
                
                return result;
            }

            return OperationResult.Fail("Идентификатор пользователя не найден, вы точно авторизованы?");
        }

        return OperationResult.Fail("Пожалуйста, проверьте, всё ли верно заполнено.");
    }

    [HttpDelete]
    public async Task<OperationResult> DeleteToken(Guid id)
    {
        if (id == Guid.Empty)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return OperationResult.Fail("Похоже что идентификатор токена отсутствует");
        }

        return await _tokensService.DeleteTokenAsync(id);
    }
}
