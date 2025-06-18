namespace Shared.Configuration;

public static class App
{
    public static string Name => "eDice";
    public static string VersionDotNet => $"{typeof(Version).Assembly.GetName().Version}";
    public static string VersionApp => "0.0.0.1";
}