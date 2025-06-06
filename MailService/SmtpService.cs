using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailService;

public class SmtpService : ISmtpService
{
    private readonly SmtpSettings _smtpSettings;
    
    public SmtpService(IOptions<SmtpSettings> smtpOptions)
    {
        _smtpSettings = smtpOptions.Value;
    }
    
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        // Тема письма
        message.Subject = subject;
        
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        message.Body = bodyBuilder.ToMessageBody();
        
        using var smtpClient = new SmtpClient();
        try
        {
            var socketOption = _smtpSettings.EnableSsl 
                ? SecureSocketOptions.StartTls 
                : SecureSocketOptions.Auto;
            
            await smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, socketOption);
            await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

            // Отправляем письмо
            await smtpClient.SendAsync(message);
        }
        catch
        {
            throw;
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }
    
}