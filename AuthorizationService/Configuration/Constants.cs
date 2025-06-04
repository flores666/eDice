namespace AuthorizationService.Configuration;

public static class Constants
{
    public static TimeSpan RestorePasswordDelay = TimeSpan.FromMinutes(30);
    public static TimeSpan ConfirmEmailDelay = TimeSpan.FromMinutes(30);
    public static TimeSpan RestoreCodeTimeAlive = TimeSpan.FromMinutes(15);
}
