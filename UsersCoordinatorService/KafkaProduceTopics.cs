using Shared.MessageBus.Kafka;

namespace UsersCoordinatorService;

public class KafkaProduceTopics
{
    public required KafkaConsumerOptions Emails { get; set; }
}