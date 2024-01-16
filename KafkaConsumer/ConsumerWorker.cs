using Confluent.Kafka;
using Newtonsoft.Json;
using SharedClasses;

namespace WorkerService1;

public class ConsumerWorker : BackgroundService
{
    private readonly ILogger<ConsumerWorker> _logger;

    public ConsumerWorker(ILogger<ConsumerWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "order-consumer-group",
        };

        using var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();

        try
        {
            consumer.Subscribe("orders");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);

                var message = JsonConvert.DeserializeObject<OrderRequest>(
                    consumeResult.Message.Value
                );
                _logger.LogDebug($"Consumed message: {message.CustomerName}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error consuming messages");
        }
        finally
        {
            consumer.Close();
        }
    }
}
