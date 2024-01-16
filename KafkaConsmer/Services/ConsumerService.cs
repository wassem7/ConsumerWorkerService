using Confluent.Kafka;
using Newtonsoft.Json;
using SharedClasses;

namespace KafkaConsmer.Services;

public class ConsumerService:IConsumerService
{
    private readonly string bootstrapserver = "localhost:9092";
    private readonly string topic = "orders";
    private readonly string groupid = "order-consumer-group-1";

    
    public async Task StartConsumingAsync(CancellationTokenSource cancellationToken)
    {
        try
        {
            var config = new ConsumerConfig()
            {
                BootstrapServers = bootstrapserver,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = groupid
            };

            using var consumer = new ConsumerBuilder<Null, string>(config).Build();
            consumer.Subscribe(topic);
            while (true)
            {
                var response = consumer.Consume(cancellationToken.Token);

                if (response.Message is not null)
                {
                    var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(
                        response.Message.Value
                    );
                    Console.WriteLine($"Order Id - {orderRequest.Id}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
