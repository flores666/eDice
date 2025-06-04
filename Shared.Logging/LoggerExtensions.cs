using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.Logging;

public static class LoggerExtensions
{
    public static IHostBuilder UseLogger(this IHostBuilder hostBuilder)
    {
        return hostBuilder
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                // Настраиваем стандартный провайдер логирования:
                services.AddLogging(service =>
                {
                    service.ClearProviders();
                    service.AddConfiguration(configuration.GetSection("Logging"));
                    service.AddConsole();
                    service.AddDebug();
                });

                // Регистрируем нашу обёртку:
                services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            });
    } 
}