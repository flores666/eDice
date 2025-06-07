namespace Shared.Logging;

/// <summary>
/// Интерфейс для логирования 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAppLogger<T> 
{
    bool IsInformationEnabled { get; }
    bool IsDebugEnabled { get; }
    void LogTrace(string message, params object[] args);
    void LogDebug(string message, params object[] args);
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
    void LogCritical(Exception exception, string message, params object[] args);
}