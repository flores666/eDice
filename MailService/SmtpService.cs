using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.Logging;

namespace MailService;

public class SmtpService : ISmtpService
{
    private readonly IAppLogger<SmtpService> _logger;
    private readonly SmtpSettings _smtpSettings;

    public SmtpService(IOptions<SmtpSettings> smtpOptions, IAppLogger<SmtpService> logger)
    {
        _logger = logger;
        _smtpSettings = smtpOptions.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (_logger.IsDebugEnabled)
            _logger.LogDebug("SmtpService -> Sending email");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();
        try
        {
            await smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);

            if (_logger.IsDebugEnabled)
                _logger.LogDebug("SmtpService -> Connected to Gmail SMTP server");

            await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

            if (_logger.IsDebugEnabled)
                _logger.LogDebug("SmtpService -> Authenticated to Gmail SMTP server");

            await smtpClient.SendAsync(message);

            if (_logger.IsDebugEnabled)
                _logger.LogDebug("SmtpService -> Message sent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SmtpService -> Error sending email");
        }
        finally
        {
            if (smtpClient.IsConnected)
                await smtpClient.DisconnectAsync(true);
        }
    }
}