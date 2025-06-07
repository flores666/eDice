using Microsoft.Extensions.DependencyInjection;
using Shared.MessageBus.Kafka.Consumer;
using Shared.MessageBus.Kafka.Producer;

namespace Shared.MessageBus.Kafka;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaProducer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(IMessagesProducer<>), typeof(KafkaMessagesProducer<>));
        return serviceCollection;
    }

    public static IServiceCollection AddKafkaConsumer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(KafkaMessagesConsumer<>));
        return serviceCollection;
    }
}
