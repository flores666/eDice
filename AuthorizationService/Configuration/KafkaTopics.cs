namespace AuthorizationService.Configuration;

public static class KafkaTopics
{
    public static string UserCreated => "UserCreated";
    public static string EmailConfirmRequested => "EmailConfirmRequested";
    public static string PasswordResetRequested => "PasswordResetRequested";
}