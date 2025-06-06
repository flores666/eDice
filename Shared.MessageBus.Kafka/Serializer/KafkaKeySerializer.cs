using System.Text.Json;
using Confluent.Kafka;

namespace Shared.MessageBus.Kafka.Serializer;

public class KafkaKeySerializer : ISerializer<Guid>
{
    public byte[] Serialize(Guid data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}