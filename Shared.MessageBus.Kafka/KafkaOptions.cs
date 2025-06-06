namespace Shared.MessageBus.Kafka;

public static class KafkaOptions
{
    public static string BootstrapServers => Environment.GetEnvironmentVariable("KAFKA__BOOTSTRAP_SERVERS") ?? "";
}
