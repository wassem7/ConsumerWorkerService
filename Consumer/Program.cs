using Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        // services.AddHostedService<KafkaConsumerWorker>();
        // services.AddSingleton<IHostedService, KafkaConsumerWorker>();
    })
    .Build();

await host.RunAsync();
