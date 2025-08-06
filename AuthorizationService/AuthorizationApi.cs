using AuthorizationService.Configuration;
using AuthorizationService.Helpers;
using AuthorizationService.Models;
using AuthorizationService.Models.Tokens;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Auth;
using Shared.Lib.Extensions;
using Shared.Models;

namespace AuthorizationService;

public static class AuthorizationApi
{
    public static IEndpointRouteBuilder MapAuthorizationApi(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/login", Login).RequireAuthorization("DenyAuthenticated");
        builder.MapPost("/register", Register).RequireAuthorization("DenyAuthenticated");
        
        builder.MapPost("/restore", RequestRestorePassword);
        builder.MapPost("/restore/{code}", RestorePassword);
        
        builder.MapPost("/confirm", RequestConfirm);
        builder.MapPost("/confirm/{code}", Confirm);

        builder.MapPost("/refresh", RefreshTokens);
        builder.MapPost("/logout", Logout).RequireAuthorization();
        
        return builder;
    }

    [DenyAuthenticated]
    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        IAuthorizationManager authorizationManager,
        HttpContext httpContext)
    {
        var response = new OperationResult();
        
        if (!ModelValidator.TryValidateObject(request, out var messages))
        {
            response.Message = messages.FirstOrDefault();
            return Results.BadRequest(response);
        }

        request.UserIp = httpContext.GetUserIp();
        response = await authorizationManager.AuthenticateAsync(request);
        if (response.IsSuccess)
        {
            var token = (response.Data as TokenResultModel)?.RefreshToken;
            
            if (!string.IsNullOrEmpty(token)) SetRefreshTokenCookie(httpContext, token);
            
            return Results.Ok(response);
        }

        return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
    }

    [DenyAuthenticated]
    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        IAuthorizationManager authorizationManager)
    {
        var response = new OperationResult();
        
        if (!ModelValidator.TryValidateObject(request, out var messages))
        {
            response.Message = messages.FirstOrDefault();
            return Results.BadRequest(response);
        }

        response = await authorizationManager.RegisterAsync(request);
        if (response.IsSuccess) return Results.Ok(response);

        return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
    }
    
    [DenyAuthenticated]
    private static async Task<IResult> RequestRestorePassword(
        [FromBody] RequestRestorePasswordRequest request,
        IAuthorizationManager authorizationManager)
    {
        var response = new OperationResult();
        
        if (!ModelValidator.TryValidateObject(request, out var messages))
        {
            response.Message = messages.FirstOrDefault();
            return Results.BadRequest(response);
        }
        
        response = await authorizationManager.CreateRestorePasswordRequestAsync(request);
        if (response.IsSuccess) return Results.Ok(response);

        return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
    }
    
    [DenyAuthenticated]
    private static async Task<IResult> RestorePassword(
        IAuthorizationManager authorizationManager, 
        [FromBody] RestorePasswordRequest request,
        string code)
    {
        var response = new OperationResult();
        if (string.IsNullOrEmpty(code))
        {
            response.Message = "Код для восстановления пароля не найден";
            return Results.BadRequest(response);
        }

        request.Code = code;
        response = await authorizationManager.RestorePasswordAsync(request);
        if (response.IsSuccess) return Results.Ok(response);

        return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
    }

    [DenyAuthenticated]
    private static async Task<IResult> Confirm(string code, IAuthorizationManager authorizationManager)
    {
        var response = new OperationResult();
        if (string.IsNullOrEmpty(code))
        {
            response.Message = "Код для восстановления пароля не найден";
            return Results.BadRequest(response);
        }

        response = await authorizationManager.ConfirmEmailAsync(code);
        if (response.IsSuccess) return Results.Ok(response);
        
        return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
    }
    
    [DenyAuthenticated]
    private static async Task<IResult> RequestConfirm(IAuthorizationManager authorizationManager, [FromBody] RequestEmailConfirmRequest request)
    {
        var response = new OperationResult();
        
        if (!ModelValidator.TryValidateObject(request, out var messages))
        {
            response.Message = messages.FirstOrDefault();
            return Results.BadRequest(response);
        }
        
        response = await authorizationManager.CreateConfirmEmailRequestAsync(request);
        if (response.IsSuccess) return Results.Ok(response);
        
        return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
    }
    
    private static async Task<IResult> Logout(IAuthorizationManager authorizationManager, HttpContext context)
    {
        if (!context.Request.Cookies.TryGetValue("rt", out var refreshToken)) return Results.Unauthorized();
        
        var request = new RefreshTokenRequest(refreshToken, context.GetUserIp());
        
        var response = await authorizationManager.LogoutAsync(request);
        if (response.IsSuccess)
        {
            context.Response.Cookies.Delete("rt");
            return Results.Ok(response);
        }
        
        return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
    }
    
    private static async Task<IResult> RefreshTokens(IAuthorizationManager authorizationManager, HttpContext context)
    {
        if (!context.Request.Cookies.TryGetValue("rt", out var refreshToken)) return Results.Unauthorized();
        
        var request = new RefreshTokenRequest(refreshToken, context.GetUserIp());
        
        var response = await authorizationManager.RefreshTokenAsync(request);
        if (response.IsSuccess)
        {
            var token = (response.Data as TokenResultModel)?.RefreshToken;
            
            if (!string.IsNullOrEmpty(token)) SetRefreshTokenCookie(context, token);
            
            return Results.Ok(response);
        }
        
        return Results.Json(response, statusCode: StatusCodes.Status500InternalServerError);
    }
    
    private static void SetRefreshTokenCookie(HttpContext httpContext, string token)
    {
        httpContext.Response.Cookies.Append("rt", token, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.Add(Constants.RefreshTokenLifeTime),
            Path = "/",
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        });
    }
}
