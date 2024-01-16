using Confluent.Kafka;
using Newtonsoft.Json;
using SharedClasses;

namespace ConsumerWorkerService;

public class Consumer : BackgroundService
{
    private readonly ILogger<Consumer> _logger;

    public Consumer(ILogger<Consumer> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig()
        {
            GroupId = "order-consumers-group",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var consumerBuilder = new ConsumerBuilder<Null, string>(config).Build();
        using var consumer = consumerBuilder;
        consumer.Subscribe("orders");
        var token = new CancellationTokenSource();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var response = consumer.Consume(token.Token);

                if (response.Message != null)
                {
                    var message = JsonConvert.DeserializeObject<OrderRequest>(
                        response.Message.Value
                    );

                    _logger.LogInformation($"consumed Order - {message.CustomerName}");
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message.ToString());
            }
        }
    }
}
