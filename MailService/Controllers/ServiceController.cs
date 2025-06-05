namespace MailService;

public static class ServiceController
{
    public static IEndpointRouteBuilder MapServiceBuilder(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("/service");

        groupBuilder.MapGet("/info", Info);

        return groupBuilder;
    }

    private static IResult Info()
        => Results.Ok(new
        {
            service = "MailService",
            description = "Почтовый сервис для отправки уведомлений!"
        });
}