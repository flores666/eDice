using Microsoft.Extensions.Logging;

namespace Shared.Logging;

public class AppLogger<T>(ILogger<T> logger) : IAppLogger<T>
{
    public bool IsInformationEnabled => logger.IsEnabled(LogLevel.Information);
    public bool IsDebugEnabled => logger.IsEnabled(LogLevel.Debug);
    public void LogTrace(string message, params object[] args) => logger.LogTrace(message, args);

    public void LogDebug(string message, params object[] args) => logger.LogDebug(message, args);

    public void LogInformation(string message, params object[] args) => logger.LogInformation(message, args);

    public void LogWarning(string message, params object[] args) => logger.LogWarning(message, args);

    public void LogError(Exception exception, string message, params object[] args) => logger.LogError(exception, message, args);

    public void LogCritical(Exception exception, string message, params object[] args) => logger.LogCritical(exception, message, args);
}