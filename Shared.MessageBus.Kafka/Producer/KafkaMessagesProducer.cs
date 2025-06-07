using Confluent.Kafka;
using Shared.MessageBus.Kafka.Serializer;

namespace Shared.MessageBus.Kafka.Producer;

public class KafkaMessagesProducer<TMessage> : IMessagesProducer<TMessage>
{
    private readonly IProducer<Guid, TMessage> _producer;

    public KafkaMessagesProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = KafkaOptions.BootstrapServers
        };

        _producer = new ProducerBuilder<Guid, TMessage>(config)
            .SetValueSerializer(new KafkaValueSerializer<TMessage>())
            .SetKeySerializer(new KafkaKeySerializer())
            .Build();
    }
    
    public async Task<Guid> PublishAsync(string topic, TMessage message, CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();
        await _producer.ProduceAsync(topic, new Message<Guid, TMessage>
        {
            Key = key,
            Value = message,
            Timestamp = new Timestamp(DateTime.UtcNow)
        }, cancellationToken);

        return key;
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}