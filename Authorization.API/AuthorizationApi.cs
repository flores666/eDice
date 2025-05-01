namespace Authorization.API;

public static class AuthorizationApi
{
    public static IEndpointRouteBuilder MapAuthorizationApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", () => "Hello");
        
        return builder;
    }
}
