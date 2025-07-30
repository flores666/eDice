using Shared.MessageBus.Kafka;

namespace UsersCoordinatorService;

public class KafkaConsumeTopics
{
    public required KafkaConsumerOptions UserRegistered { get; set; }
}