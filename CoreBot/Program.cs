await Host.CreateDefaultBuilder(args)
    .ConfigureServices((services) =>
    {
        services.AddHostedService<LogConsumer>();
        services.AddHostedService<FormatlogConsumer>();
        services.AddHostedService<ChatConsumer>();

        services.AddHostedService<LogProducer>();
        services.AddHostedService<FormatlogProducer>();
        services.AddHostedService<ChatProducer>();
        
        services.AddHostedService<LicenseControl>();

        services.AddSingleton<IBaseCache, RoleCache>();

        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new FormatlogTaskQueue(10_000);
        });

        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new LogTaskQueue(10_000);
        });

        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            return new ChatTaskQueue(10_000);
        });

        services.AddSingleton<CoreLicense>();
        services.AddSingleton(_ => JsonConvert.DeserializeObject<Definitions>(File.ReadAllText("./Configurations/Definitions.json")));              
        services.AddSingleton(_ => JsonConvert.DeserializeObject<ServerConnection>(File.ReadAllText("./Configurations/ServerConnection.json")));

        services.AddSingleton<IDiscordService, DiscordService>();
        services.AddScoped<IServerRepository, ServerRepository>();
        services.AddSingleton<ILogModelsFactory, LogModelsFactory>();                              
    })
    .Build()
    .RunAsync();