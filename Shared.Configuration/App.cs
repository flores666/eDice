namespace Shared.Configuration;

public class App
{
    public static string Name => "";
    public static string VersionDotNet => $"{typeof(Version).Assembly.GetName().Version}";
    public static string VersionApp => "0.0.0.1";
}