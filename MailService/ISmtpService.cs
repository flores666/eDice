namespace MailService;

public interface ISmtpService
{
    Task SendEmailAsync(string to, string subject, string body);
}