using System.Text.Json;
using Confluent.Kafka;
using Shared.Logging;

namespace Shared.MessageBus.Kafka.Deserializer;

public class KafkaValueDeserializer<TMessage> : IDeserializer<TMessage>
{
    private readonly IAppLogger<KafkaValueDeserializer<TMessage>> _logger;

    public KafkaValueDeserializer(IAppLogger<KafkaValueDeserializer<TMessage>> logger)
    {
        _logger = logger;
    }
    
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (_logger.IsDebugEnabled) _logger.LogDebug("Deserializing message");
        
        if (isNull || data.IsEmpty)
        {
            _logger.LogWarning("Kafka message value is null or empty.");
        }

        try
        {
            var result = JsonSerializer.Deserialize<TMessage>(data);
            if (result == null)
            {
                _logger.LogWarning($"Deserialization returned null for type {typeof(TMessage).Name}");
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Failed to deserialize message to {typeof(TMessage).Name}", ex);
            return default;
        }
    }
}