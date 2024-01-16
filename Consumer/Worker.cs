using Confluent.Kafka;
using Newtonsoft.Json;
using SharedClasses;

namespace Consumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string topic = "orders";
    private readonly string groupId = "order-consumer-group";
    private readonly string bootstrapServers = "localhost:9092";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig()
        {
            GroupId = groupId,
            BootstrapServers = bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var consumerBuilder = new ConsumerBuilder<Null, string>(config).Build();
        using var consumer = consumerBuilder;
        consumer.Subscribe(topic);
        var token = new CancellationTokenSource();
        var response = consumer.Consume(token.Token);
        // var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(response.Message.Value);
        while (true)
        {
            _logger.LogInformation("Hi from the consumer");
            await Task.Delay(1000, stoppingToken);
        }
    }
}
