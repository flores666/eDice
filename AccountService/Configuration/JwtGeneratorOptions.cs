namespace AccountService.Configuration;

public static class JwtGeneratorOptions
{
    public static string SecretKey => Environment.GetEnvironmentVariable("JWT__SECRET_KEY") ?? "";
    public static string Issuer => Environment.GetEnvironmentVariable("JWT__ISSUER") ?? "";
    public static string Audience => Environment.GetEnvironmentVariable("JWT__AUDIENCE") ?? "";
}
