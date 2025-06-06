using System.Text.Json;
using Confluent.Kafka;

namespace Shared.MessageBus.Kafka.Serializer;

public class KafkaValueSerializer<TMessage> : ISerializer<TMessage>
{
    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}