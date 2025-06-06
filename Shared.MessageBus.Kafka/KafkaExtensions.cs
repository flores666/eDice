using Microsoft.Extensions.DependencyInjection;
using Shared.MessageBus.Kafka.Producer;

namespace Shared.MessageBus.Kafka;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafka(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(IMessageProducer<>), typeof(KafkaMessageProducer<>));
        return serviceCollection;
    }
}
