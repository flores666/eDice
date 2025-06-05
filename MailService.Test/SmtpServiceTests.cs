using MailService.Models;
using Microsoft.Extensions.Options;

namespace MailService.Test;

public class Tests
{
    private readonly string _to = "dimaassdf5@gmail.com";
    private readonly string _subject = "Test subject";
    private readonly string _body = "Test body";
    
    private IOptions<SmtpSettings> CreateOptions()
    {
        var settings = new SmtpSettings
        {
            Host = "smtp.gmail.com",        // например, Gmail SMTP
            Port = 587,
            EnableSsl = true,
            Username = "ttestov006@gmail.com",
            Password = "frai uyld xgrl ywhe", // для Gmail нужно использовать App Password
            FromEmail = "info@dndapp.com",
            FromName = "Dnd app"
        };

        return Options.Create(settings);
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SendMail_CallsEmailSender()
    {
        var smtpService = new SmtpService(CreateOptions());
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await smtpService.SendEmailAsync(_to, _subject, _body);
        });
    }
}