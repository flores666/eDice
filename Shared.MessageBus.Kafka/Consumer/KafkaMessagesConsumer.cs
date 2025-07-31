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

    public KafkaMessagesConsumer(IOptionsMonitor<KafkaConsumerOptions> consumerOptions, IMessagesHandler<TMessage> handler,
        IAppLogger<KafkaMessagesConsumer<TMessage>> logger, IAppLogger<KafkaValueDeserializer<TMessage>> deserializerLogger)
    {
        _logger = logger;
        _handler = handler;
        
        _logger.LogInformation("TMessage = {T}", typeof(TMessage).Name);
        
        var options = consumerOptions.Get(typeof(TMessage).Name);
        var config = new ConsumerConfig
        {
            BootstrapServers = KafkaOptions.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = options.GroupId
        };

        _topic = options.Topic;

        _consumer = new ConsumerBuilder<string, TMessage>(config)
            .SetValueDeserializer(new KafkaValueDeserializer<TMessage>(deserializerLogger))
            .Build();
        
        _logger.LogInformation("Consumer initialized with group: {G} and topic: {T}", options.GroupId, options.Topic);
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Executing background task");
        return Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken);
    }

    private async Task ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);
        _logger.LogInformation("Subscribed to topic: {T}", _topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                if (_logger.IsDebugEnabled) _logger.LogDebug("Consumed message with key {M}", result.Message.Key);
                await _handler.HandleAsync(result.Message.Value, stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.ToString());
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping background task");
        _consumer.Close();
        return base.StopAsync(cancellationToken);
    }
}