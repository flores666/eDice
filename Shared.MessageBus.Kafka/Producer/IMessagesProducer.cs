namespace Shared.MessageBus.Kafka.Producer;

public interface IMessagesProducer<in TMessage> : IDisposable
{
    public Task<Guid> PublishAsync(string topic, TMessage message, CancellationToken cancellationToken = default);
}