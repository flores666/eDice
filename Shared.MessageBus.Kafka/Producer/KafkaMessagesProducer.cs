using Confluent.Kafka;
using Shared.Logging;
using Shared.MessageBus.Kafka.Serializer;

namespace Shared.MessageBus.Kafka.Producer;

public class KafkaMessagesProducer<TMessage> : IMessagesProducer<TMessage>
{
    private readonly IAppLogger<KafkaMessagesProducer<TMessage>> _logger;
    private readonly IProducer<Guid, TMessage> _producer;

    public KafkaMessagesProducer(IAppLogger<KafkaMessagesProducer<TMessage>> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = KafkaOptions.BootstrapServers
        };

        _producer = new ProducerBuilder<Guid, TMessage>(config)
            .SetValueSerializer(new KafkaValueSerializer<TMessage>())
            .SetKeySerializer(new KafkaKeySerializer())
            .Build();
        
        _logger.LogInformation("Ready!");
    }
    
    public async Task<Guid> PublishAsync(string topic, TMessage message, CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();
        
        if (_logger.IsDebugEnabled) _logger.LogDebug("Published message with key {K}", key);
        
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
        if (_logger.IsDebugEnabled) _logger.LogDebug("Disposed");
        _producer?.Dispose();
    }
}