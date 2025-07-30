using Microsoft.Extensions.Options;
using Shared.MessageBus.Kafka.MessagesHandler;
using Shared.MessageBus.Kafka.Producer;
using UsersCoordinatorService.MessageModels;

namespace UsersCoordinatorService.Handlers.UserRegistration;

public class UserRegisteredHandler : IMessagesHandler<UserMessage>
{
    private readonly IMessagesProducer<EmailMessage> _producer;
    private readonly IOptions<KafkaProduceTopics> _config;

    public UserRegisteredHandler(IMessagesProducer<EmailMessage> producer, IOptions<KafkaProduceTopics> config)
    {
        _producer = producer;
        _config = config;
    }
    
    public async Task HandleAsync(UserMessage message, CancellationToken cancellationToken)
    {
        var frontUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
        if (string.IsNullOrEmpty(frontUrl)) throw new Exception("Frontend URL is empty");
        if (string.IsNullOrEmpty(message.ResetCode)) throw new ArgumentNullException(nameof(message.ResetCode));
        
        var uploadMessage = new EmailMessage
        {
            To = message.Email,
            Subject = "Подтверждение адреса электронной почты",
            Body = EmailTemplates.GetConfirmEmailBody(frontUrl, message.ResetCode)
        };

        await _producer.PublishAsync(_config.Value.Emails.Topic, uploadMessage, cancellationToken);
    }
}