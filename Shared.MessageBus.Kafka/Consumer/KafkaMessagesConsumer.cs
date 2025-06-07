using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Logging;
using Shared.MessageBus.Kafka.Deserializer;
using Shared.MessageBus.Kafka.MessagesHandler;

namespace Shared.MessageBus.Kafka.Consumer;

public class KafkaMessagesConsumer<TMessage> : BackgroundService
{
    private readonly IAppLogger<KafkaMessagesConsumer<TMessage>> _logger;
    private readonly IMessagesHandler<TMessage> _handler;
    private readonly string _topic;
    private readonly IConsumer<string, TMessage> _consumer;

    public KafkaMessagesConsumer(IOptions<KafkaConsumerOptions> consumerOptions, 
        IAppLogger<KafkaMessagesConsumer<TMessage>> logger, IMessagesHandler<TMessage> handler)
    {
        _logger = logger;
        _handler = handler;
        var config = new ConsumerConfig
        {
            BootstrapServers = KafkaOptions.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = consumerOptions.Value.GroupId
        };

        _topic = consumerOptions.Value.Topic;

        _consumer = new ConsumerBuilder<string, TMessage>(config)
            .SetValueDeserializer(new KafkaValueDeserializer<TMessage>())
            .Build();
        
        _logger.LogInformation("Ready!");
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Executing background task");
        return Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken);
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                if (_logger.IsInformationEnabled) _logger.LogDebug("Consumed message with key {M}", result.Message.Key);
                await _handler.HandleAsync(result.Message.Value, stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.ToString());
        }
    }
}