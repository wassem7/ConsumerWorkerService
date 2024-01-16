using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // services.AddHostedService<Worker>();
        services.AddHostedService<ConsumerWorker>();
    })
    .Build();

await host.RunAsync();
