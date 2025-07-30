using AuthorizationService.Repository;
using AuthorizationService.Services;
using Infrastructure.AuthorizationService;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuthorizationService;

public static class Extensions
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<PostgresContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
        serviceCollection.AddScoped<IAuthorizationManager, AuthorizationManager>();
        serviceCollection.AddScoped<IUsersRepository, PostgresUsersRepository>();
        serviceCollection.AddScoped<ITokensRepository, PostgresTokensRepository>();
        
        return serviceCollection;
    }
}