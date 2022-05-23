using CoreBot.Domain.Factories;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((services) =>
    {
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new FormatlogTaskQueue(10_000);
        });

        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new LogTaskQueue(10_000);
        });

        services.AddSingleton<CoreLicense>();
        services.AddSingleton(_ => JsonConvert.DeserializeObject<Definitions>(File.ReadAllText("./Configurations/Definitions.json")));              
        services.AddSingleton(_ => JsonConvert.DeserializeObject<ServerConnection>(File.ReadAllText("./Configurations/ServerConnection.json")));
        services.AddScoped<IServerRepository, ServerRepository>();
        services.AddSingleton<ILogModelsFactory, LogModelsFactory>();        
        services.AddHostedService<LicenseControl>();
        services.AddHostedService<FormatlogProducer>();
        services.AddHostedService<LogProducer>();
        services.AddHostedService<FormatlogConsumer>();
        services.AddHostedService<LogConsumer>();       
    })
    .Build()
    .RunAsync();