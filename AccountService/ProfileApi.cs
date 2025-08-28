using AccountService.Services;
using Shared.Models;

namespace AccountService;

public static class ProfileApi
{
    public static IEndpointRouteBuilder MapProfileApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/profile/user/{id:guid}", GetUser).RequireAuthorization();
        
        return builder;
    }

    private static async Task<IResult> GetUser(Guid id, IUsersService usersService)
    {
        var response = new OperationResult();

        var user = await usersService.GetUserAsync(id);
        if (user == null)
        {
            response.Message = "Такой пользователь не найден";
        }
        else
        {
            response.Data = user;
            response.IsSuccess = true;
        }
        
        return Results.Ok(response);
    }
}
