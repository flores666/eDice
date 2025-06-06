namespace AuthorizationService.Configuration;

public static class Constants
{
    public static TimeSpan ConfirmEmailDelay = TimeSpan.FromMinutes(15);
    public static TimeSpan RestoreCodeDelay = TimeSpan.FromMinutes(15);
}
