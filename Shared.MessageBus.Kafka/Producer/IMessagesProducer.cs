namespace Shared.MessageBus.Kafka.Producer;

public interface IMessagesProducer<in TMessage> : IDisposable
{
    public Task<string> PublishAsync(string topic, TMessage message, CancellationToken cancellationToken = default);
}