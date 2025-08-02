namespace AuthorizationService.Models;

public static class OperationReasons
{
    public static string EmailConfirmRequired => "EmailConfirmRequired";
    public static string CodeTimeout => "CodeTimeout";
    public static string CodeEmailSent => "CodeEmailSent";
}