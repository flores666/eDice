using Microsoft.Extensions.Options;
using Shared.Logging;
using Shared.MessageBus.Kafka.MessagesHandler;
using Shared.MessageBus.Kafka.Producer;
using UsersCoordinatorService.Handlers.UserRegistration;
using UsersCoordinatorService.MessageModels;

namespace UsersCoordinatorService.Handlers.PasswordReset;

public class PasswordResetHandler : IMessagesHandler<PasswordResetMessage>
{
    private readonly IMessagesProducer<EmailMessage> _producer;
    private readonly IOptions<KafkaProduceTopics> _config;
    private readonly IAppLogger<PasswordResetHandler> _logger;

    public PasswordResetHandler(IMessagesProducer<EmailMessage> producer, IOptions<KafkaProduceTopics> config,
    IAppLogger<PasswordResetHandler> logger)
    {
        _producer = producer;
        _config = config;
        _logger = logger;
    }
    
    public async Task HandleAsync(PasswordResetMessage message, CancellationToken cancellationToken)
    {
        _logger.LogDebug("PasswordResetHandler -> started handling message");
        
        var frontUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
        if (string.IsNullOrEmpty(frontUrl)) throw new Exception("Frontend URL is empty");
        if (string.IsNullOrEmpty(message.ResetCode)) throw new ArgumentNullException(nameof(message.ResetCode));
        
        var uploadMessage = new EmailMessage
        {
            To = message.Email,
            Subject = "Подтверждение адреса электронной почты",
            Body = EmailTemplates.GetRestorePasswordBody(frontUrl, message.ResetCode)
        };

        await _producer.PublishAsync(_config.Value.Emails.Topic, uploadMessage, cancellationToken);
    }
}