using Confluent.Kafka;
using Newtonsoft.Json;
using SharedClasses;

namespace Consumer;

public class KafkaConsumerWorker : BackgroundService
{
    private readonly string topic = "orders";
    private readonly string groupId = "order-consumer-group";
    private readonly string bootstrapServers = "localhost:9092";

    private readonly ILogger<Worker> _logger;

    public KafkaConsumerWorker(ILogger<Worker> logger)
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

        while (true)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);

                // Process message further...
                _logger.LogInformation("Hi from the consumer");
                // Console.WriteLine("Hi from the consumer");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error consuming message");
            }
        }
    }
    // protected override Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     var config = new ConsumerConfig()
    //     {
    //         GroupId = groupId,
    //         BootstrapServers = bootstrapServers,
    //         AutoOffsetReset = AutoOffsetReset.Earliest
    //     };
    //
    //     using var consumer = new ConsumerBuilder<Null, string>(config).Build();
    //     consumer.Subscribe(topic);
    //     var cancelToken = new CancellationTokenSource();
    //
    //     while (true)
    //     {
    //         var response = consumer.Consume(cancelToken.Token);
    //         Console.WriteLine("Hi from the consumer");
    //     }
    //
    //
    // }
    // try
    // {
    //     using var consumer = new ConsumerBuilder<Null, string>(config).Build();
    //
    //     consumer.Subscribe(topic);
    //     var cancelToken = new CancellationTokenSource();
    //
    //     while (true)
    //     {
    //         var response = consumer.Consume(cancelToken.Token);
    //         var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(
    //             response.Message.Value
    //         );
    //         Console.WriteLine("hello from consumer");
    //     }
    // }
    // catch (Exception ex)
    // {
    //     Console.WriteLine(ex.Message);
    // }
}

// public Task StartAsync(CancellationToken cancellationToken)
// {
//     var config = new ConsumerConfig()
//     {
//         GroupId = groupId,
//         BootstrapServers = bootstrapServers,
//         AutoOffsetReset = AutoOffsetReset.Earliest
//     };
//
//     try
//     {
//         using var consumer = new ConsumerBuilder<Null, string>(config).Build();
//
//         consumer.Subscribe(topic);
//         var cancelToken = new CancellationTokenSource();
//
//         while (true)
//         {
//             var response = consumer.Consume(cancelToken.Token);
//             var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(
//                 response.Message.Value
//             );
//             Console.WriteLine("hello from consumer");
//         }
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex.Message);
//     }
//
//     return Task.CompletedTask;
// }

// public Task StopAsync(CancellationToken cancellationToken)
// {
//     return Task.CompletedTask;
// }
