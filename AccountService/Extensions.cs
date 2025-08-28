using AccountService.Repository;
using AccountService.Services;
using Infrastructure.AccountService;
using Microsoft.EntityFrameworkCore;

namespace AccountService;

public static class Extensions
{
    public static IServiceCollection AddAccountServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<PostgresContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
        serviceCollection.AddScoped<IAuthorizationManager, AuthorizationManager>();
        serviceCollection.AddScoped<IUsersRepository, PostgresUsersRepository>();
        serviceCollection.AddScoped<ITokensRepository, PostgresTokensRepository>();
        
        return serviceCollection;
    }
}