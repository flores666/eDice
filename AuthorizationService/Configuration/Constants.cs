namespace AuthorizationService.Configuration;

public static class Constants
{
    public static TimeSpan ConfirmEmailDelay = TimeSpan.FromMinutes(15);
    public static TimeSpan RestoreCodeDelay = TimeSpan.FromMinutes(15);
    public static TimeSpan RefreshTokenLifeTime = TimeSpan.FromDays(31);
    public static TimeSpan AccessTokenLifeTime = TimeSpan.FromMinutes(10);
}
