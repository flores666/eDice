namespace Shared.MessageBus.Kafka;

public class KafkaConsumerOptions
{
    public string GroupId { get; set; }
    public string Topic { get; set; }
}