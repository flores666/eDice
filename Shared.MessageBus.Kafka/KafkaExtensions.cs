using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.MessageBus.Kafka.Consumer;
using Shared.MessageBus.Kafka.MessagesHandler;
using Shared.MessageBus.Kafka.Producer;

namespace Shared.MessageBus.Kafka;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaProducer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(typeof(IMessagesProducer<>), typeof(KafkaMessagesProducer<>));
        return serviceCollection;
    }

    public static IServiceCollection AddKafkaConsumer<TMessage, THandler>(this IServiceCollection serviceCollection, IConfigurationSection configurationSection) 
        where THandler : class, IMessagesHandler<TMessage>
    {
        serviceCollection.Configure<KafkaConsumerOptions>(configurationSection);
        serviceCollection.AddHostedService<KafkaMessagesConsumer<TMessage>>();
        serviceCollection.AddSingleton<IMessagesHandler<TMessage>, THandler>();
        return serviceCollection;
    }
}
