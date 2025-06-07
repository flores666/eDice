namespace Shared.MessageBus.Kafka.MessagesHandler;

public interface IMessagesHandler<in TMessage>
{
    public Task HandleAsync(TMessage message, CancellationToken cancellationToken);
}