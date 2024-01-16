namespace KafkaConsmer.Services;

public interface IConsumerService
{
    public Task StartConsumingAsync(CancellationTokenSource cancellationToken);
}
