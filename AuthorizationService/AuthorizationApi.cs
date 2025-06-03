using AuthorizationService.Helpers;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService;

public static class AuthorizationApi
{
    public static IEndpointRouteBuilder MapAuthorizationApi(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/login", Login);
        builder.MapPost("/register", Register);

        return builder;
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        IAuthorizationManager authorizationManager)
    {
        if (!ModelValidator.TryValidateObject(request, out var messages))
        {
            return Results.BadRequest(messages);
        }

        var response = await authorizationManager.AuthenticateAsync(request);
        if (response.IsSuccess) return Results.Ok(response);

        return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        IAuthorizationManager authorizationManager)
    {
        if (!ModelValidator.TryValidateObject(request, out var messages))
        {
            return Results.BadRequest(messages);
        }

        var response = await authorizationManager.RegisterAsync(request);
        if (response.IsSuccess) return Results.Ok(response);

        return Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
    }
}
