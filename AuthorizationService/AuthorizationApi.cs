using Authorization.API.Helpers;
using Authorization.API.Models;
using Authorization.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.API;

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
        if (ModelValidator.TryValidateObject(request, out var messages))
        {
            return Results.BadRequest(messages);
        }

        var response = await authorizationManager.AuthenticateAsync(request);
        if (response.IsSuccess) return Results.Ok();

        return Results.Unauthorized();
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        IAuthorizationManager authorizationManager)
    {
        if (ModelValidator.TryValidateObject(request, out var messages))
        {
            return Results.BadRequest(messages);
        }

        var response = await authorizationManager.RegisterAsync(request);
        if (response.IsSuccess) return Results.Ok();

        return Results.Unauthorized();
    }
}
