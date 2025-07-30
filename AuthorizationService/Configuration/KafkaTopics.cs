namespace AuthorizationService.Configuration;

public static class KafkaTopics
{
    public static string UserCreated => "users.registered";
    public static string EmailConfirmRequested => "users.emailConfirmRequested";
    public static string PasswordResetRequested => "users.passwordResetRequested";
}