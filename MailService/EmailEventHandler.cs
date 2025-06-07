using MailService.Models;
using Shared.Logging;
using Shared.MessageBus.Kafka.MessagesHandler;

namespace MailService;

public class EmailEventHandler : IMessagesHandler<EmailRequest>
{
    private readonly ISmtpService _smtpService;
    private readonly IAppLogger<EmailEventHandler> _logger;

    public EmailEventHandler(ISmtpService smtpService, IAppLogger<EmailEventHandler> logger)
    {
        _smtpService = smtpService;
        _logger = logger;
    }
    
    public async Task HandleAsync(EmailRequest message, CancellationToken cancellationToken)
    {
        if (_logger.IsDebugEnabled) _logger.LogDebug("Executing email event handler");
        await _smtpService.SendEmailAsync(message.To, message.Subject, message.Body);
    }
}